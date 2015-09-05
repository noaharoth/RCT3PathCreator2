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

	public value struct MPathSection
	{
	public:
		property String^ Section;

		MPathSection(String^ fileName);

	internal:

		RCT3Asset::PathSection Convert();

	};

	public ref class MPath
	{
	public:
		property String^ Name;
		property String^ IngameName;
		property String^ Icon;
		property String^ TextureA;
		property String^ TextureB;
		property String^ Shared;
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

		property unsigned int Unknown01;
		property unsigned int Unknown02;
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
		property MPathSection Paving;

		#pragma endregion

		MPath();

		void CopyFilesTo(String^ destination);
	
		void CreateTextureOVL(String^ path, MOutputLog^ log);

		void CreateIconOVL(String^ path, MOutputLog^ log);

		void CreateStubOVL(String^ path, MOutputLog^ log);

		void CreateBlankOVL(String^ path, MOutputLog^ log);

	};
}