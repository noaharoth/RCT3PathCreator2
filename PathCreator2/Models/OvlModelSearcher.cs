// OvlModelSearcher.cs

/*
* (C) Copyright 2015 Noah Roth
*
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the GNU Lesser General Public License
* (LGPL) version 2.1 which accompanies this distribution, and is available at
* http://www.gnu.org/licenses/lgpl-2.1.html
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
* Lesser General Public License for more details.
*/

using PathCreator.Models;
using R3ALInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace PathCreator
{
    /// <summary>
    /// Struct returned by OvlModelSearcher.Search function.
    /// </summary>
    public struct OvlModelSearchResult
    {
        private PathCreatorProjectType _type;
        private int _modelsFound;
        private int _totalModels;
        private List<string> _remainingOvlModels;

        public OvlModelSearchResult(PathCreatorProjectType type, int modelsFound, int totalModels, List<string> remainingOvlModels)
        {
            _type = type;
            _modelsFound = modelsFound;
            _totalModels = totalModels;
            _remainingOvlModels = remainingOvlModels;
        }

        /// <summary>
        /// Remaining OVL models that were not found.
        /// </summary>
        public List<string> RemainingOvlModels { get { return _remainingOvlModels; } }

        /// <summary>
        /// The number of models found.
        /// </summary>
        public int ModelsFound { get { return _modelsFound; } }

        /// <summary>
        /// The total number of models searched for.
        /// </summary>
        public int TotalModels { get { return _totalModels; } }

        /// <summary>
        /// Checks if all models were found.
        /// </summary>
        /// <returns>True if ModelsFound equals TotalModels.</returns>
        public bool AllModelsFound()
        {
            return !(_modelsFound < _totalModels);
        }

        /// <summary>
        /// Converts the RemainingOvlModels List to a string, each model formatted on its own line.
        /// </summary>
        /// <returns>List of all remaining models in the form of a string.</returns>
        public string GetRemainingOvlModelsString()
        {
            string remaining = "";

            foreach (string model in _remainingOvlModels)
            {
                remaining += model + "\n";
            }

            return remaining;
        }

        /// <summary>
        /// Summarizes the OvlModelSearchResult information in a neatly formatted WPF MessageBox.
        /// </summary>
        public void ShowResultAsMessageBox()
        {
            string msgBoxMessage = null;
            string msgBoxCaption = null;
            MessageBoxImage msgBoxImage = MessageBoxImage.Information;

            switch (_type)
            {
                case PathCreatorProjectType.Queue:

                    if (AllModelsFound())
                    {
                        msgBoxMessage = $"All { _totalModels } queue model OVLs were found & loaded.";
                        msgBoxCaption = "All models found!";
                    }
                    else
                    {
                        msgBoxMessage = $"{ _modelsFound } out of the required { _totalModels } queue model OVLs were found & loaded.\n\nModels not found:\n{ GetRemainingOvlModelsString() }";
                        msgBoxCaption = "Missing queue models!";
                        msgBoxImage = MessageBoxImage.Warning;
                    }

                    break;
                case PathCreatorProjectType.BasicPath:

                    if (AllModelsFound())
                    {
                        msgBoxMessage = $"All { _totalModels } of the required path model OVLs were found & loaded.";
                        msgBoxCaption = "All models found!";
                    }
                    else
                    {
                        msgBoxMessage = $"{ _modelsFound } out of the required { _totalModels } path model OVLs were found & loaded.\n\nModels not found:\n{ GetRemainingOvlModelsString() }";
                        msgBoxCaption = "Missing path models!";
                        msgBoxImage = MessageBoxImage.Warning;
                    }

                    break;
                case PathCreatorProjectType.ExtendedPath:

                    if (AllModelsFound())
                    {
                        msgBoxMessage = $"All { _totalModels } of both the required & optional path model OVLs were found & loaded.";
                        msgBoxCaption = "All models found!";
                    }
                    else if (_modelsFound >= OvlModelSearcher.PathModelCount)
                    {
                        msgBoxMessage = $"All { OvlModelSearcher.PathModelCount } of the required path model OVLs were found & loaded.\n\nMissing extended models (OPTIONAL):\n{ GetRemainingOvlModelsString() }";
                        msgBoxCaption = "All required models found!";
                    }
                    else
                    {
                        msgBoxMessage = $"{ _modelsFound } out of the required { OvlModelSearcher.PathModelCount } path model OVLs were found & loaded.\n\nModels not found (including OPTIONAL extended models):\n{ GetRemainingOvlModelsString() }";
                        msgBoxCaption = "Missing path models!";
                        msgBoxImage = MessageBoxImage.Warning;
                    }

                    break;
            }

            MessageBox.Show(msgBoxMessage, msgBoxCaption, MessageBoxButton.OK, msgBoxImage);
        }

    }

    /// <summary>
    /// Class that searches through a given directory to find OVL files that follow
    /// RCT3 Path/Queue model naming conventions.
    /// </summary>
    public class OvlModelSearcher
    {
        private Regex _regex = null;
        private Dictionary<string, Action<string>> _ovlModels = null;
        private List<string> _remainingOvlModels = null;
        private MQueue _queue = null;
        private MPath _path = null;
        private int _found = 0;

        public const int QueueModelCount = 7;
        public const int PathModelCount = 19;
        public const int ExtPathModelCount = PathModelCount + 17;

        public const string RegexPattern = @"[^_]+_([^_]+)\.common\.ovl";

        public OvlModelSearcher(MQueue queueObject, MPath pathObject)
        {
            if (queueObject == null && pathObject == null)
                throw new ArgumentNullException("Either queueObject or pathObject must be initialized");

            _queue = queueObject;
            _path = pathObject;

            _regex = new Regex(RegexPattern, RegexOptions.IgnoreCase);

            Reset();
        }

        /// <summary>
        /// Resets the OvlModelSearcher object so another search can be executed.
        /// </summary>
        public void Reset()
        {
            if (_queue != null)
            {

                if (_ovlModels == null)
                    _ovlModels = new Dictionary<string, Action<string>>(QueueModelCount);
                else
                    _ovlModels.Clear();

                if (_remainingOvlModels == null)
                    _remainingOvlModels = new List<string>(QueueModelCount);
                else
                    _remainingOvlModels.Clear();

                _remainingOvlModels.Add("Straight");
                _remainingOvlModels.Add("TurnL");
                _remainingOvlModels.Add("TurnR");
                _remainingOvlModels.Add("SlopeUp");
                _remainingOvlModels.Add("SlopeDown");
                _remainingOvlModels.Add("SlopeStraight");

                _ovlModels.Add("Straight", ovl => { _queue.Straight = ovl; _found++; _remainingOvlModels.Remove("Straight"); });
                _ovlModels.Add("TurnL", ovl => { _queue.TurnL = ovl; _found++; _remainingOvlModels.Remove("TurnL"); });
                _ovlModels.Add("TurnR", ovl => { _queue.TurnR = ovl; _found++; _remainingOvlModels.Remove("TurnR"); });
                _ovlModels.Add("SlopeUp", ovl => { _queue.SlopeUp = ovl; _found++; _remainingOvlModels.Remove("SlopeUp"); });
                _ovlModels.Add("SlopeDown", ovl => { _queue.SlopeDown = ovl; _found++; _remainingOvlModels.Remove("SlopeDown"); });
                _ovlModels.Add("SlopeStraight", ovl => { _queue.SlopeStraight1 = ovl; _found++; _remainingOvlModels.Remove("SlopeStraight"); });
            }
            else
            {

                if (_path.IsExtended)
                {
                    if (_ovlModels == null)
                        _ovlModels = new Dictionary<string, Action<string>>(ExtPathModelCount);
                    else
                        _ovlModels.Clear();

                    if (_remainingOvlModels == null)
                        _remainingOvlModels = new List<string>(ExtPathModelCount);
                    else
                        _remainingOvlModels.Clear();
                }
                else
                {
                    if (_ovlModels == null)
                        _ovlModels = new Dictionary<string, Action<string>>(PathModelCount);
                    else
                        _ovlModels.Clear();

                    if (_remainingOvlModels == null)
                        _remainingOvlModels = new List<string>(PathModelCount);
                    else
                        _remainingOvlModels.Clear();
                }

                _remainingOvlModels.Add("Flat");
                _remainingOvlModels.Add("StraightA");
                _remainingOvlModels.Add("StraightB");
                _remainingOvlModels.Add("CornerA");
                _remainingOvlModels.Add("CornerB");
                _remainingOvlModels.Add("CornerC");
                _remainingOvlModels.Add("CornerD");
                _remainingOvlModels.Add("TurnU");
                _remainingOvlModels.Add("TurnLA");
                _remainingOvlModels.Add("TurnLB");
                _remainingOvlModels.Add("TurnTA");
                _remainingOvlModels.Add("TurnTB");
                _remainingOvlModels.Add("TurnTC");
                _remainingOvlModels.Add("TurnX");
                _remainingOvlModels.Add("Slope");
                _remainingOvlModels.Add("SlopeStraight");
                _remainingOvlModels.Add("SlopeStraightLeft");
                _remainingOvlModels.Add("SlopeStraightRight");
                _remainingOvlModels.Add("SlopeMid");

                _ovlModels.Add("Flat", ovl => { _path.Flat = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("Flat"); });
                _ovlModels.Add("StraightA", ovl => { _path.StraightA = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("StraightA"); });
                _ovlModels.Add("StraightB", ovl => { _path.StraightB = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("StraightB"); });
                _ovlModels.Add("CornerA", ovl => { _path.CornerA = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("CornerA"); });
                _ovlModels.Add("CornerB", ovl => { _path.CornerB = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("CornerB"); });
                _ovlModels.Add("CornerC", ovl => { _path.CornerC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("CornerC"); });
                _ovlModels.Add("CornerD", ovl => { _path.CornerD = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("CornerD"); });
                _ovlModels.Add("TurnU", ovl => { _path.TurnU = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnU"); });
                _ovlModels.Add("TurnLA", ovl => { _path.TurnLA = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnLA"); });
                _ovlModels.Add("TurnLB", ovl => { _path.TurnLB = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnLB"); });
                _ovlModels.Add("TurnTA", ovl => { _path.TurnTA = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnTA"); });
                _ovlModels.Add("TurnTB", ovl => { _path.TurnTB = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnTB"); });
                _ovlModels.Add("TurnTC", ovl => { _path.TurnTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnTC"); });
                _ovlModels.Add("TurnX", ovl => { _path.TurnX = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("TurnX"); });
                _ovlModels.Add("Slope", ovl => { _path.Slope = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("Slope"); });
                _ovlModels.Add("SlopeStraight", ovl => { _path.SlopeStraight = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraight"); });
                _ovlModels.Add("SlopeStraightLeft", ovl => { _path.SlopeStraightL = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightLeft"); });
                _ovlModels.Add("SlopeStraightRight", ovl => { _path.SlopeStraightR = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightRight"); });
                _ovlModels.Add("SlopeMid", ovl => { _path.SlopeMid = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeMid"); });

                if (_path.IsExtended)
                {
                    _remainingOvlModels.Add("FlatFC");
                    _remainingOvlModels.Add("SlopeFC");
                    _remainingOvlModels.Add("SlopeBC");
                    _remainingOvlModels.Add("SlopeTC");
                    _remainingOvlModels.Add("SlopeStraightFC");
                    _remainingOvlModels.Add("SlopeStraightBC");
                    _remainingOvlModels.Add("SlopeStraightTC");
                    _remainingOvlModels.Add("SlopeStraightLeftFC");
                    _remainingOvlModels.Add("SlopeStraightLeftBC");
                    _remainingOvlModels.Add("SlopeStraightLeftTC");
                    _remainingOvlModels.Add("SlopeStraightRightFC");
                    _remainingOvlModels.Add("SlopeStraightRightBC");
                    _remainingOvlModels.Add("SlopeStraightRightTC");
                    _remainingOvlModels.Add("SlopeMidFC");
                    _remainingOvlModels.Add("SlopeMidBC");
                    _remainingOvlModels.Add("SlopeMidTC");
                    _remainingOvlModels.Add("Paving");

                    _ovlModels.Add("FlatFC", ovl => { _path.FlatFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("FlatFC"); });
                    _ovlModels.Add("SlopeFC", ovl => { _path.SlopeFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeFC"); });
                    _ovlModels.Add("SlopeBC", ovl => { _path.SlopeBC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeBC"); });
                    _ovlModels.Add("SlopeTC", ovl => { _path.SlopeTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeTC"); });
                    _ovlModels.Add("SlopeStraightFC", ovl => { _path.SlopeStraightFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightFC"); });
                    _ovlModels.Add("SlopeStraightBC", ovl => { _path.SlopeStraightBC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightBC"); });
                    _ovlModels.Add("SlopeStraightTC", ovl => { _path.SlopeStraightTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightTC"); });
                    _ovlModels.Add("SlopeStraightLeftFC", ovl => { _path.SlopeStraightLFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightLeftFC"); });
                    _ovlModels.Add("SlopeStraightLeftBC", ovl => { _path.SlopeStraightLBC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightLeftBC"); });
                    _ovlModels.Add("SlopeStraightLeftTC", ovl => { _path.SlopeStraightLTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightLeftTC"); });
                    _ovlModels.Add("SlopeStraightRightFC", ovl => { _path.SlopeStraightRFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightRightFC"); });
                    _ovlModels.Add("SlopeStraightRightBC", ovl => { _path.SlopeStraightRBC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightRightBC"); });
                    _ovlModels.Add("SlopeStraightRightTC", ovl => { _path.SlopeStraightRTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeStraightRightTC"); });
                    _ovlModels.Add("SlopeMidFC", ovl => { _path.SlopeMidFC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeMidFC"); });
                    _ovlModels.Add("SlopeMidBC", ovl => { _path.SlopeMidBC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeMidBC"); });
                    _ovlModels.Add("SlopeMidTC", ovl => { _path.SlopeMidTC = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("SlopeMidTC"); });
                    _ovlModels.Add("Paving", ovl => { _path.Paving = new MPathSection(ovl); _found++; _remainingOvlModels.Remove("Paving"); });

                }
            }
        }

        /// <summary>
        /// Searches a directory to find OVL model files which follow the Regex pattern.
        /// </summary>
        /// <param name="directory">The directory to search.</param>
        /// <returns>OvlModelSearchResult object.</returns>
        public OvlModelSearchResult Search(string directory)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            string[] fileNames = Directory.GetFiles(directory, "*.common.ovl");

            Match match;

            foreach (string fileName in fileNames)
            {
                match = _regex.Match(fileName);

                if (match.Success && _ovlModels.ContainsKey(match.Groups[1].Value))
                {
                    _ovlModels[match.Groups[1].Value](fileName);
                }
            }

            int totalToBeFound = _ovlModels.Count;

            int found = _found;

            PathCreatorProjectType type = PathCreatorProjectType.Queue;

            if (_path != null)
            {
                if (_path.IsExtended)
                    type = PathCreatorProjectType.ExtendedPath;
                else
                    type = PathCreatorProjectType.BasicPath;
            }

            return new OvlModelSearchResult(type, found, totalToBeFound, _remainingOvlModels);
        }

    }
}
