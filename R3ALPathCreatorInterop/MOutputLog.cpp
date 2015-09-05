// MOutputLog.cpp

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

#include "MOutputLog.hpp"

using namespace R3ALInterop;

#pragma region OvlErrorEventArgs

OvlErrorEventArgs::OvlErrorEventArgs(String^ message) 
	: _message(message)
{
}

// Returns the OVL error message.
String^ OvlErrorEventArgs::Message()
{
	return _message;
}

#pragma endregion

#pragma region NativeErrorCallbackHandler

NativeErrorCallbackHandler::NativeErrorCallbackHandler(MOutputLog^ owner) 
	: _owner(owner)
{
	_owner->Native().AssignCallback(&OnError, this);
}

void NativeErrorCallbackHandler::OnError(RCT3Debugging::OutputLog& sender, std::string& message, void* userData)
{
	NativeErrorCallbackHandler* handler = static_cast<NativeErrorCallbackHandler*>(userData);
	handler->_owner->RaiseErrorEvent(gcnew String(message.c_str(), 0, message.length()));
}

#pragma endregion

#pragma region MOutputLog

MOutputLog::MOutputLog() 
	: _outputLogInternal(new RCT3Debugging::OutputLog()), _callbackHandler(new NativeErrorCallbackHandler(this))
{
}

MOutputLog::~MOutputLog()
{
	delete _outputLogInternal;
	_outputLogInternal = nullptr;
	delete _callbackHandler;
	_callbackHandler = nullptr;

	GC::SuppressFinalize(this);
}

MOutputLog::!MOutputLog()
{
	this->~MOutputLog();
}

void MOutputLog::EnableDebugging()
{
	_outputLogInternal->EnableDebugging();
}

void MOutputLog::Debug(String^ message)
{
	_outputLogInternal->Debug(util::std_string(message));
}

void MOutputLog::Info(String^ message)
{
	_outputLogInternal->Info(util::std_string(message));
}

void MOutputLog::Warning(String^ message)
{
	_outputLogInternal->Warning(util::std_string(message));
}

void MOutputLog::Error(String^ message)
{
	_outputLogInternal->Error(util::std_string(message));
}

void MOutputLog::SaveToFile(String^ fileName)
{
	_outputLogInternal->SaveToFile(util::std_string(fileName));
}

unsigned int MOutputLog::GetErrorCount()
{
	return _outputLogInternal->ErrorCount();
}

String^ MOutputLog::GetErrors()
{
	return marshal_as<String^>(_outputLogInternal->GetErrors());
}

void MOutputLog::RaiseErrorEvent(String^ message)
{
	ErrorEvent(this, message);
}

RCT3Debugging::OutputLog& MOutputLog::Native()
{
	return *_outputLogInternal;
}

#pragma endregion