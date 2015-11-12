// MOutoutLog.hpp
// Managed wrapper class for OutputLog

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

#include <OutputLog.hpp>

#include "System.hpp"
#include "Utilities.hpp"

namespace R3ALInterop
{

	ref class MOutputLog;

	// If provided, will get called when the first error is
	// reached.
	public delegate void OvlErrorEventHandler(MOutputLog^ sender, String^ message);

	class NativeErrorCallbackHandler
	{
	private:
		gcroot<MOutputLog^> _owner;

		static void OnError(RCT3Debugging::OutputLog& sender, std::string& message, void* userData);

	public:
		NativeErrorCallbackHandler(MOutputLog^ owner);
	};

	// Wrapper class for RCT3Debugging::OutputLog.
	public ref class MOutputLog
	{
	private:

		RCT3Debugging::OutputLog* _outputLogInternal;
		NativeErrorCallbackHandler* _callbackHandler;
		OvlErrorEventHandler^ _managedHandler;

	public:

		// Constructor.
		MOutputLog();

		// Dispose
		~MOutputLog();

		// Finalizer
		!MOutputLog();

		// Enables debugging messages to be logged.
		void EnableDebugging();

		// Adds a debug message to the OutputLog.
		void Debug(String^ message);

		// Adds an information message to the OutputLog.
		void Info(String^ message);

		// Adds a warning message to the OutputLog.
		void Warning(String^ message);

		// Adds an error message to the OutputLog.
		// If an ErrorCallback delegate was provided, it will be
		// invoked upon reaching the first error.
		void Error(String^ message);

		// Saves the OutputLog to the specified file.
		void SaveToFile(String^ fileName);

		// Returns the amount of errors.
		unsigned int GetErrorCount();

		// Returns the list of errors.
		String^ GetErrors();

		#pragma region OvlErrorEventHandler

		event OvlErrorEventHandler^ ErrorEvent
		{
			[MethodImplAttribute(MethodImplOptions::Synchronized)]
			void add(OvlErrorEventHandler^ value)
			{
				_managedHandler = safe_cast<OvlErrorEventHandler^>(Delegate::Combine(value, _managedHandler));
			}

			[MethodImplAttribute(MethodImplOptions::Synchronized)]
			void remove(OvlErrorEventHandler^ value)
			{
				_managedHandler = safe_cast<OvlErrorEventHandler^>(Delegate::Remove(value, _managedHandler));
			}

		private:

			void raise(MOutputLog^ sender, String^ message)
			{
				OvlErrorEventHandler^ handler = _managedHandler;
				if (handler != nullptr)
					handler(this, message);
			}
		}

		#pragma endregion

	internal:

		void RaiseErrorEvent(String^ message);

		// Returns reference to native OutputLog class.
		RCT3Debugging::OutputLog& Native();

	};
}