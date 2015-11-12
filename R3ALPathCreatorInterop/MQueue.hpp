// MQueue.hpp
// Class for creating RCT3 custom queues using the RCT3AssetLibrary

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

#include <Queue.hpp>
#include <FlexiTexture.hpp>
#include <Texture.hpp>
#include <FlicManager.hpp>
#include <SceneryItem.hpp>

#include "System.hpp"
#include "Utilities.hpp"
#include "MOutputLog.hpp"

namespace R3ALInterop
{

	// Managed wrapper class for RCT3Asset::Queue class.
	public ref class MQueue
	{
	public:
		property String^ Name;
		property String^ IngameName;
		property String^ Icon;
		property String^ Texture;
		property String^ Shared; // Path to shared texture OVL, is not required for creating paths
		property String^ Straight;
		property String^ TurnL;
		property String^ TurnR;
		property String^ SlopeUp;
		property String^ SlopeDown;
		property String^ SlopeStraight1;
		property String^ SlopeStraight2;
		property bool Recolor1;
		property bool Recolor2;
		property bool Recolor3;

		// Constructor.
		MQueue();

		// Copies queue model OVL files to the specified destination.
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