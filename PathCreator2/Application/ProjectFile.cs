// ProjectFile.cs

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

using R3ALInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace PathCreator
{
    /// <summary>
    /// Holds information about a Path Creator project, which can be saved and loaded.
    /// </summary>
    public class ProjectFile
    {

        #region File constants

        private const ushort Signature = 0x5043; // "CP"

        private const uint OldSignature = 0x46464844;

        private const uint Version = 2;

        public const string Extension = ".cpath";

        #endregion

        private Project _project;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="project">The PathCreatorProject object that will be saved</param>
        public ProjectFile(Project project)
        {
            _project = project;
        }

        /// <summary>
        /// PathCreatorProject object supplied by the PathCreatorProjectFile constructor.
        /// </summary>
        public Project Project {  get { return _project; } }

        /// <summary>
        /// Saves the PathCreatorProjectFile to the specified destination.
        /// </summary>
        /// <param name="destination">The destination to save to.</param>
        public void Save(string destination)
        {
            if (destination == null)
                throw new ArgumentNullException("path");

            if (_project == null)
                throw new NullReferenceException("PathCreatorProject object is null");

            try
            {
                using (BinaryWriter w = new BinaryWriter(File.OpenWrite(destination), Encoding.ASCII))
                {
                    w.Write(Signature);
                    w.Write(Version);
                    w.Write(_project.ProjectName);
                    w.Write((ushort)_project.ProjectType);

                    if (_project.ProjectType == ProjectType.Queue)
                    {
                        MQueue queue = _project.QueueObject;
                        w.Write(queue.Name);
                        w.Write(queue.IngameName);
                        w.Write(queue.Icon);
                        w.Write(queue.Texture);
                        w.Write(queue.Shared);
                        w.Write(queue.Straight);
                        w.Write(queue.TurnL);
                        w.Write(queue.TurnR);
                        w.Write(queue.SlopeUp);
                        w.Write(queue.SlopeDown);
                        w.Write(queue.SlopeStraight1);
                        w.Write(queue.SlopeStraight2);
                        w.Write(queue.Recolor1);
                        w.Write(queue.Recolor2);
                        w.Write(queue.Recolor3);
                    }
                    else
                    {
                        MPath path = _project.PathObject;
                        w.Write(path.Name);
                        w.Write(path.IngameName);
                        w.Write(path.Icon);
                        w.Write(path.TextureA);
                        w.Write(path.TextureB);
                        w.Write(path.Shared);
                        w.Write(path.Flat.Section);
                        w.Write(path.StraightA.Section);
                        w.Write(path.StraightB.Section);
                        w.Write(path.CornerA.Section);
                        w.Write(path.CornerB.Section);
                        w.Write(path.CornerC.Section);
                        w.Write(path.CornerD.Section);
                        w.Write(path.TurnU.Section);
                        w.Write(path.TurnLA.Section);
                        w.Write(path.TurnLB.Section);
                        w.Write(path.TurnTA.Section);
                        w.Write(path.TurnTB.Section);
                        w.Write(path.TurnTC.Section);
                        w.Write(path.TurnX.Section);
                        w.Write(path.Slope.Section);
                        w.Write(path.SlopeStraight.Section);
                        w.Write(path.SlopeStraightL.Section);
                        w.Write(path.SlopeStraightR.Section);
                        w.Write(path.SlopeMid.Section);
                        w.Write(path.UnderwaterSupport);
                        w.Write(path.IsExtended);

                        if (path.IsExtended)
                        {
                            w.Write(path.Unknown01);
                            w.Write(path.Unknown02);
                            w.Write(path.FlatFC.Section);
                            w.Write(path.SlopeFC.Section);
                            w.Write(path.SlopeBC.Section);
                            w.Write(path.SlopeTC.Section);
                            w.Write(path.SlopeStraightFC.Section);
                            w.Write(path.SlopeStraightBC.Section);
                            w.Write(path.SlopeStraightTC.Section);
                            w.Write(path.SlopeStraightLFC.Section);
                            w.Write(path.SlopeStraightLBC.Section);
                            w.Write(path.SlopeStraightLTC.Section);
                            w.Write(path.SlopeStraightRFC.Section);
                            w.Write(path.SlopeStraightRBC.Section);
                            w.Write(path.SlopeStraightRTC.Section);
                            w.Write(path.SlopeMidFC.Section);
                            w.Write(path.SlopeMidBC.Section);
                            w.Write(path.SlopeMidTC.Section);
                            w.Write(path.Paving.Section);
                        }
                    }
                }

                _project.ProjectFilePath = destination;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to save project \"{ _project.ProjectName }\": { e.Message }");
            }
        }

        /// <summary>
        /// Opens the specified Path Creator project and returns a PathCreatorProjectFile.
        /// </summary>
        /// <param name="fileName">The path to the project file to load</param>
        /// <returns>The PathCreatorProjectFile object. Null if an error occurred.</returns>
        public static ProjectFile Open(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (File.Exists(fileName) == false)
                throw new FileNotFoundException($"{fileName} does not exist!");

            Project project = null;
            ProjectFile projectFile = null;

            BinaryReader r = null;

            try
            {
                using (r = new BinaryReader(File.OpenRead(fileName), Encoding.ASCII))
                {
                    ushort signature;
                    uint version;

                    signature = r.ReadUInt16();

                    if (signature != Signature)
                    {
                        // See if it's the old CPATH file
                        r.BaseStream.Seek(0, SeekOrigin.Begin);

                        uint oldSignature = r.ReadUInt32();

                        if (oldSignature == OldSignature)
                        {
                            MessageBoxResult result = MessageBox.Show("This project was saved with an older version of the RCT3 Path Creator and is" +
                                " not backwards-compatible. Click 'OK' to upgrade the project file to the new format.",
                                "Upgrade Project?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                            if (result == MessageBoxResult.OK)
                            {
                                r.BaseStream.Seek(0, SeekOrigin.Begin);
                                UpgradeOldCpathFile(r, fileName);
                                Open(fileName);
                            }
                            else
                            {
                                r.Close();
                                r.Dispose();
                                return null;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Unable to load file \"{ fileName }\": it is not a valid Path Creator project or is corrupted.");
                            r.Close();
                            r.Dispose();
                            return null;
                        }
                    }

                    version = r.ReadUInt32();

                    if (version != Version)
                    {
                        if (IsVersionSupported(version))
                        {
                            // TO-DO: Call function that will read appropriate version file format
                        }
                        else
                        {
                            MessageBox.Show($"Unable to load file \"{ fileName }\": version not supported.");
                            r.Close();
                            r.Dispose();
                            return null;
                        }
                    }

                    string projectName = r.ReadString();
                    ProjectType projectType = (ProjectType)r.ReadUInt16();

                    project = new Project();
                    project.Initialize(projectName, projectType);
                    project.ProjectFilePath = fileName;

                    projectFile = new ProjectFile(project);


                    if (projectType == ProjectType.Queue)
                    {
                        MQueue queue = new MQueue();
                        project.QueueObject = queue;

                        queue.Name = r.ReadString();
                        queue.IngameName = r.ReadString();
                        queue.Icon = r.ReadString();
                        queue.Texture = r.ReadString();
                        queue.Shared = r.ReadString();
                        queue.Straight = r.ReadString();
                        queue.TurnL = r.ReadString();
                        queue.TurnR = r.ReadString();
                        queue.SlopeUp = r.ReadString();
                        queue.SlopeDown = r.ReadString();
                        queue.SlopeStraight1 = r.ReadString();
                        queue.SlopeStraight2 = r.ReadString();
                        queue.Recolor1 = r.ReadBoolean();
                        queue.Recolor2 = r.ReadBoolean();
                        queue.Recolor3 = r.ReadBoolean();
                    }
                    else
                    {
                        MPath path = new MPath();
                        project.PathObject = path;

                        path.Name = r.ReadString();
                        path.IngameName = r.ReadString();
                        path.Icon = r.ReadString();
                        path.TextureA = r.ReadString();
                        path.TextureB = r.ReadString();
                        path.Shared = r.ReadString();
                        path.Flat = new MPathSection(r.ReadString());
                        path.StraightA = new MPathSection(r.ReadString());
                        path.StraightB = new MPathSection(r.ReadString());
                        path.CornerA = new MPathSection(r.ReadString());
                        path.CornerB = new MPathSection(r.ReadString());
                        path.CornerC = new MPathSection(r.ReadString());
                        path.CornerD = new MPathSection(r.ReadString());
                        path.TurnU = new MPathSection(r.ReadString());
                        path.TurnLA = new MPathSection(r.ReadString());
                        path.TurnLB = new MPathSection(r.ReadString());
                        path.TurnTA = new MPathSection(r.ReadString());
                        path.TurnTB = new MPathSection(r.ReadString());
                        path.TurnTC = new MPathSection(r.ReadString());
                        path.TurnX = new MPathSection(r.ReadString());
                        path.Slope = new MPathSection(r.ReadString());
                        path.SlopeStraight = new MPathSection(r.ReadString());
                        path.SlopeStraightL = new MPathSection(r.ReadString());
                        path.SlopeStraightR = new MPathSection(r.ReadString());
                        path.SlopeMid = new MPathSection(r.ReadString());
                        path.UnderwaterSupport = r.ReadBoolean();
                        path.IsExtended = r.ReadBoolean();

                        if (path.IsExtended)
                        {
                            path.Unknown01 = r.ReadUInt32();
                            path.Unknown02 = r.ReadUInt32();
                            path.FlatFC = new MPathSection(r.ReadString());
                            path.SlopeFC = new MPathSection(r.ReadString());
                            path.SlopeBC = new MPathSection(r.ReadString());
                            path.SlopeTC = new MPathSection(r.ReadString());
                            path.SlopeStraightFC = new MPathSection(r.ReadString());
                            path.SlopeStraightBC = new MPathSection(r.ReadString());
                            path.SlopeStraightTC = new MPathSection(r.ReadString());
                            path.SlopeStraightLFC = new MPathSection(r.ReadString());
                            path.SlopeStraightLBC = new MPathSection(r.ReadString());
                            path.SlopeStraightLTC = new MPathSection(r.ReadString());
                            path.SlopeStraightRFC = new MPathSection(r.ReadString());
                            path.SlopeStraightRBC = new MPathSection(r.ReadString());
                            path.SlopeStraightRTC = new MPathSection(r.ReadString());
                            path.SlopeMidFC = new MPathSection(r.ReadString());
                            path.SlopeMidBC = new MPathSection(r.ReadString());
                            path.SlopeMidTC = new MPathSection(r.ReadString());
                            path.Paving = new MPathSection(r.ReadString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error reading project file: { e.Message }", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                if (r != null)
                {
                    r.Close();
                    r.Dispose();
                }
            }

            return projectFile;
        }

        /// <summary>
        /// List of supported project file versions that can be loaded.
        /// </summary>
        public static List<uint> SupportedVersions = new List<uint>() { 2 };

        /// <summary>
        /// Checks to see if the supplied version can be loaded.
        /// </summary>
        /// <param name="version">The version number to search for</param>
        /// <returns>True if the version is supported</returns>
        public static bool IsVersionSupported(uint version)
        {
            if (SupportedVersions.Contains(version))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Upgrades the CPATH file format from Path Creator versions previous to 2.1
        /// </summary>
        /// <param name="r">The stream to the file</param>
        /// <param name="fileName">The name of the file</param>
        private static void UpgradeOldCpathFile(BinaryReader r, string fileName)
        {
            // The old file format. Such a pain in the butt to read, not sure why I had to make it so unnecessarily complex...
        }

    }

}
