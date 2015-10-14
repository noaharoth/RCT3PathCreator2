using R3ALInterop;
using System.IO;
using System.Text.RegularExpressions;

namespace PathCreator.Models
{

    /// <summary>
    /// The type of Path Creator project.
    /// </summary>
    public enum PathCreatorProjectType
    {
        BasicPath,
        ExtendedPath,
        Queue
    }

    /// <summary>
    /// Class that contains a Path/Queue object.
    /// </summary>
    public class PathCreatorProject
    {
        /// <summary>
        /// The name of the Path Creator project. Not the name of the path/queue!
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The type of Path Creator project.
        /// </summary>
        public PathCreatorProjectType ProjectType { get; set; }

        /// <summary>
        /// The path of the saved project file.
        /// If project has not been saved, this property is
        /// null.
        /// </summary>
        public string ProjectFilePath { get; set; }

        /// <summary>
        /// The MPath class object. If project is a queue, then this property is
        /// null.
        /// </summary>
        public MPath PathObject { get; set; }

        /// <summary>
        /// The MQueue class object. If project is a path, then this property is
        /// null.
        /// </summary>
        public MQueue QueueObject { get; set; }

        #region Common properties

        /// <summary>
        /// The internal name of either the PathObject or QueueObject, depending on project type.
        /// Only letters, numbers, underscore, and dash is allowed.
        /// </summary>
        public string InternalName
        {
            get
            {
                if (PathObject != null)
                    return PathObject.Name;
                else
                    return QueueObject.Name;
            }

            set
            {
                if (PathObject != null)
                    PathObject.Name = value;
                else
                    QueueObject.Name = value;
            }
        }

        /// <summary>
        /// The in-game name of either the PathObject or QueueObject, depending on project type.
        /// </summary>
        public string IngameName
        {
            get
            {
                if (PathObject != null)
                    return PathObject.IngameName;
                else
                    return QueueObject.IngameName;
            }

            set
            {
                if (PathObject != null)
                    PathObject.IngameName = value;
                else
                    QueueObject.IngameName = value;
            }
        }

        /// <summary>
        /// The path to the icon image of either the PathObject or QueueObject, depending on project type.
        /// </summary>
        public string IconPath
        {
            get
            {
                if (PathObject != null)
                    return PathObject.Icon;
                else
                    return QueueObject.Icon;
            }

            set
            {
                if (PathObject != null)
                    PathObject.Icon = value;
                else
                    QueueObject.Icon = value;
            }
        }

        /// <summary>
        /// The path to the shared texture common OVL of either the PathObject or QueueObject, depending on project type.
        /// </summary>
        public string SharedPath
        {
            get
            {
                if (PathObject != null)
                    return PathObject.Shared;
                else
                    return QueueObject.Shared;
            }

            set
            {
                if (PathObject != null)
                    PathObject.Shared = value;
                else
                    QueueObject.Shared = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectType">Type of the project.</param>
        public PathCreatorProject(string projectName, PathCreatorProjectType projectType)
        {
            ProjectName = projectName;
            ProjectType = projectType;
            ProjectFilePath = null;

            if (projectType == PathCreatorProjectType.Queue)
            {
                QueueObject = new MQueue();
                PathObject = null;
            }
            else
            {
                PathObject = new MPath();
                QueueObject = null;
            }

        }

        #region Private functions

        /// <summary>
        /// Performs a shallow error check in the MPath class.
        /// </summary>
        /// <param name="errorList">String that will contain the list of errors found, otherwise will be null.</param>
        /// <returns>Number of errors.</returns>
        private int shallowPathCheck(out string errorList)
        {
            int errorCount = 0;
            errorList = "";

            if (!File.Exists(PathObject.TextureA))
            {
                errorCount++;
                errorList += $"Error { errorCount }: TextureA not specified or file doesn't exist.";
            }

            if (!File.Exists(PathObject.TextureB))
            {
                errorCount++;
                errorList += $"Error { errorCount }: TextureB not specified or file doesn't exist.";
            }

            if (!File.Exists(PathObject.Flat.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'Flat' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.StraightA.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'StraightA' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.StraightB.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'StraightB' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.CornerA.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'CornerA' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.CornerB.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'CornerB' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.CornerC.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'CornerC' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.CornerD.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'CornerD' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnU.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnU' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnLA.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnLA' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnLB.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnLB' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnTA.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnTA' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnTB.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnTB' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnTC.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnTC' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.TurnX.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnX' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.Slope.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'Slope' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.SlopeStraight.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeStraight' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.SlopeStraightL.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeStraightL' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.SlopeStraightR.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeStraightR' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(PathObject.SlopeMid.Section))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeMid' model OVL not specified or OVL doesn't exist.";
            }

            if (PathObject.IsExtended)
            {

                if (!string.IsNullOrWhiteSpace(PathObject.FlatFC.Section) && !File.Exists(PathObject.FlatFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'FlatFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeFC.Section) && !File.Exists(PathObject.SlopeFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeBC.Section) && !File.Exists(PathObject.SlopeBC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeBC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeTC.Section) && !File.Exists(PathObject.SlopeTC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeTC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightFC.Section) && !File.Exists(PathObject.SlopeStraightFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightBC.Section) && !File.Exists(PathObject.SlopeStraightBC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightBC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightTC.Section) && !File.Exists(PathObject.SlopeStraightTC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightTC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightLFC.Section) && !File.Exists(PathObject.SlopeStraightLFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightLFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightLBC.Section) && !File.Exists(PathObject.SlopeStraightLBC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightLBC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightLTC.Section) && !File.Exists(PathObject.SlopeStraightLTC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightLTC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightRFC.Section) && !File.Exists(PathObject.SlopeStraightRFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightRFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightRBC.Section) && !File.Exists(PathObject.SlopeStraightRBC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightRBC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeStraightRTC.Section) && !File.Exists(PathObject.SlopeStraightRTC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeStraightRTC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeMidFC.Section) && !File.Exists(PathObject.SlopeMidFC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeMidFC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeMidBC.Section) && !File.Exists(PathObject.SlopeMidBC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeMidBC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.SlopeMidTC.Section) && !File.Exists(PathObject.SlopeMidTC.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'SlopeMidTC' model OVL doesn't exist.\n";
                }

                if (!string.IsNullOrWhiteSpace(PathObject.Paving.Section) && !File.Exists(PathObject.Paving.Section))
                {
                    errorCount++;
                    errorList += $"Error { errorCount }: Optional 'Paving' model OVL doesn't exist.\n";
                }
            }

            return errorCount;
        }

        /// <summary>
        /// Performs a shallow error check in the MQueue class.
        /// </summary>
        /// <param name="errorList">String that will contain the list of errors found, otherwise will be null.</param>
        /// <returns>Number of errors.</returns>
        private int shallowQueueCheck(out string errorList)
        {
            int errorCount = 0;
            errorList = "";

            if (!File.Exists(QueueObject.Texture))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Texture not specified or file doesn't exist.";
            }

            if (!File.Exists(QueueObject.Straight))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'Straight' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(QueueObject.TurnL))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnL' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(QueueObject.TurnR))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'TurnR' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(QueueObject.SlopeUp))
            {
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeUp' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(QueueObject.SlopeDown))
            { 
                errorCount++;
                errorList += $"Error { errorCount }: 'SlopeDown' model OVL not specified or OVL doesn't exist.";
            }

            if (!File.Exists(QueueObject.SlopeStraight1))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Optional 'SlopeStraight1' model OVL doesn't exist.\n\tPath: '{ QueueObject.SlopeStraight1 }'\n";
            }

            if (!string.IsNullOrWhiteSpace(QueueObject.SlopeStraight2) && !File.Exists(QueueObject.SlopeStraight2))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Optional 'SlopeStraight2' model OVL doesn't exist.\n\tPath: '{ QueueObject.SlopeStraight2 }'\n";
            }

            return errorCount;
        }

        #endregion

        /// <summary>
        /// Performs a shallow error check that is required before proceeding with any OVL creation.
        /// </summary>
        /// <param name="errorList">String that will contain the list of errors found, otherwise will be null.</param>
        /// <returns>Number of errors encountered.</returns>
        public int ShallowCheck(out string errorList)
        {
            int errorCount;

            if (PathObject != null)
                errorCount = shallowPathCheck(out errorList);
            else
                errorCount = shallowQueueCheck(out errorList);

            if (string.IsNullOrWhiteSpace( InternalName ) || !Regex.IsMatch(InternalName, @"^[a-zA-Z0-9_-]+$"))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Short name not specified or contains illegal characters.\n";
            }

            if (string.IsNullOrWhiteSpace( IngameName ))
            {
                errorCount++;
                errorList += $"Error { errorCount }: In-game name not specified.\n";
            }

            if (!File.Exists(IconPath))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Icon image does not exist.\n\tPath: '{IconPath}'\n";
            }

            if (!string.IsNullOrWhiteSpace(SharedPath) && !File.Exists(SharedPath))
            {
                errorCount++;
                errorList += $"Error { errorCount }: Shared OVL (common file) does not exist.\n\tPath: '{SharedPath}'\n";
            }

            if (errorCount == 0)
                errorList = null;

            return errorCount;
        }

    }

}
