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

	public ref class MQueue
	{
	public:
		property String^ Name;
		property String^ IngameName;
		property String^ Icon;
		property String^ Texture;
		property String^ Shared;
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

		MQueue();

		void CopyFilesTo(String^ destination);

		void CreateTextureOVL(String^ path, MOutputLog^ log);

		void CreateIconOVL(String^ path, MOutputLog^ log);

		void CreateStubOVL(String^ path, MOutputLog^ log);

		void CreateBlankOVL(String^ path, MOutputLog^ log);

	};

}