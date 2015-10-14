// System.hpp

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

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <msclr/gcroot.h>

#include <OvlFile.hpp>

using namespace msclr::interop;
using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::IO;
using namespace System::Text;
using namespace System::Runtime::InteropServices;
using namespace System::Runtime::CompilerServices;

namespace R3ALInterop
{

	public ref class RCT3AssetLibrary
	{
	public:

		static void Initialize(array<wchar_t>^ args)
		{
			if (args == nullptr)
			{
				RCT3Asset::InitializeRCT3AssetLibrary(nullptr);
				return;
			}

			if (args->Length)
			{
				IntPtr p = Marshal::StringToHGlobalAnsi(gcnew String(args));
				RCT3Asset::InitializeRCT3AssetLibrary(static_cast<const char*>(p.ToPointer()));
				Marshal::FreeHGlobal(p);
			}
			else
			{
				RCT3Asset::InitializeRCT3AssetLibrary(nullptr);
			}
		}

	};

}