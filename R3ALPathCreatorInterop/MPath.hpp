// MPath.hpp

// Classes for creating RCT3 custom paths using the RCT3AssetLibrary

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

#pragma once

#include <Path.hpp>
#include <Texture.hpp>
#include <FlicManager.hpp>
#include <SceneryItem.hpp>

#include "System.hpp"
#include "Utilities.hpp"
#include "MOutputLog.hpp"

namespace R3ALInterop
{

	/// Managed wrapper class fo RCT3Asset::PathSection class.
	public value struct MPathSection
	{
	public:
		property String^ Section; // File path to model OVL file (common.ovl file)

		// Constructor.
		MPathSection(String^ fileName);

	internal:

		// Converts this managed class to its unmanaged/native counterpart.
		RCT3Asset::PathSection Convert();

	};

	// Managed wrapper class for RCT3Asset::Path class.
	public ref class MPath
	{
	public:
		property String^ Name;
		property String^ IngameName;
		property String^ Icon;
		property String^ TextureA;
		property String^ TextureB;
		property String^ Shared; // Path to shared texture OVL, is not required for creating paths
		property MPathSection Flat;
		property MPathSection StraightA;
		property MPathSection StraightB;
		property MPathSection CornerA;
		property MPathSection CornerB;
		property MPathSection CornerC;
		property MPathSection CornerD;
		property MPathSection TurnU;
		property MPathSection TurnLA;
		property MPathSection TurnLB;
		property MPathSection TurnTA;
		property MPathSection TurnTB;
		property MPathSection TurnTC;
		property MPathSection TurnX;
		property MPathSection Slope;
		property MPathSection SlopeStraight;
		property MPathSection SlopeStraightL;
		property MPathSection SlopeStraightR;
		property MPathSection SlopeMid;
		property bool UnderwaterSupport;
		property bool IsExtended;

		#pragma region Extended properties

		property unsigned int Unknown01; // Usually 0
		property unsigned int Unknown02; // Usually 1
		property MPathSection FlatFC;
		property MPathSection SlopeFC;
		property MPathSection SlopeBC;
		property MPathSection SlopeTC;
		property MPathSection SlopeStraightFC;
		property MPathSection SlopeStraightBC;
		property MPathSection SlopeStraightTC;
		property MPathSection SlopeStraightLFC;
		property MPathSection SlopeStraightLBC;
		property MPathSection SlopeStraightLTC;
		property MPathSection SlopeStraightRFC;
		property MPathSection SlopeStraightRBC;
		property MPathSection SlopeStraightRTC;
		property MPathSection SlopeMidFC;
		property MPathSection SlopeMidBC;
		property MPathSection SlopeMidTC;
		property MPathSection Paving; // Is actually just a single string in files, but I used MPathSection instead for consistency

		#pragma endregion

		// Constructor.
		MPath();

		// Copies path model OVL files to the specified destination.
		//     * Throws System::Exception-inherited classes
		void CopyFilesTo(String^ destination);
	
		// Creates the texture OVL files.
		//     * Registers errors to the MOutputLog
		void CreateTextureOVL(String^ path, MOutputLog^ log);

		// Creates the icon OVL files.
		//     * Registers errors to the MOutputLog
		void CreateIconOVL(String^ path, MOutputLog^ log);

		// Creates the stub OVL files.
		//     * Registers errors to the MOutputLog
		void CreateStubOVL(String^ path, MOutputLog^ log);

		// Creates the blank OVL files.
		//     * Registers errors to the MOutputLog
		void CreateBlankOVL(String^ path, MOutputLog^ log);

	};
}