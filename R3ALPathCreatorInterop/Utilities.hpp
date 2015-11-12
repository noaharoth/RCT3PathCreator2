// Utilities.hpp

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

#include "System.hpp"

class util
{

public:

	static String^ GetOvlName(String^ fileName)
	{
		array<String^>^ file = fileName->Split('\\');

		return file[file->Length - 1]->Split('.')[0];
	}

	static void CopyOvlFile(String^ ovlFileName, String^ destinationDirectory)
	{
		String^ common = ovlFileName;
		String^ unique = common->Replace("common.ovl", "unique.ovl");

		/*if (!File::Exists(common))
			throw gcnew R3ALInterop::OvlException(String::Format("File \"{0}\" does not exist!", common));

		if (!File::Exists(unique))
			throw gcnew R3ALInterop::OvlException(String::Format("File \"{0}\" does not exist!", unique));*/

		if (!File::Exists(common))
			throw gcnew System::IO::FileNotFoundException(common);

		if (!File::Exists(unique))
			throw gcnew System::IO::FileNotFoundException(unique);

		if (!Directory::Exists(destinationDirectory))
			throw gcnew System::IO::DirectoryNotFoundException(destinationDirectory);

		File::Copy(common, destinationDirectory + Path::GetFileName(common));
		File::Copy(unique, destinationDirectory + Path::GetFileName(unique));
	}

	__forceinline static void CopyOvlFileOptional(String^ ovlFileName, String^ destinationDirectory)
	{
		if (!String::IsNullOrWhiteSpace(ovlFileName))
			CopyOvlFile(ovlFileName, destinationDirectory);
	}

	__forceinline static std::string GetOvlName_std(String^ fileName)
	{
		return marshal_as<std::string>(GetOvlName(fileName));
	}

	__forceinline static std::string std_string(String^ str)
	{
		return marshal_as<std::string>(str);
	}

	__forceinline static std::wstring std_wstring(String^ str)
	{
		return marshal_as<std::wstring>(str);
	}

};