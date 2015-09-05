// MQueue.cpp

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

#include "MQueue.hpp"

using namespace R3ALInterop;

#pragma region Macros

#define DOQUEUESECTION(QSECTION) sid.Name(qtd.QSECTION); \
	sid.TxtName = text; \
	sid.GsiIcon = qtd.GsiIcon; \
	sid.OvlPath = ovlPath + qtd.QSECTION; \
	sid.Svds.push_back(qtd.QSECTION + ":svd"); \
	sidCol.Add(sid); \
	sid.Svds.clear();

#pragma endregion

#pragma region MQueue

MQueue::MQueue()
{
	Name = "";
	IngameName = "";
	Icon = "";
	Texture = "";
	Shared = "";
	Straight = "";
	TurnL = "";
	TurnR = "";
	SlopeUp = "";
	SlopeDown = "";
	SlopeStraight1 = "";
	SlopeStraight2 = "";
	Recolor1 = false;
	Recolor2 = false;
	Recolor3 = false;
}

void MQueue::CopyFilesTo(String^ destination)
{
	util::CopyOvlFile(Straight, destination);
	util::CopyOvlFile(TurnL, destination);
	util::CopyOvlFile(TurnR, destination);
	util::CopyOvlFile(SlopeUp, destination);
	util::CopyOvlFile(SlopeDown, destination);
	util::CopyOvlFile(SlopeStraight1, destination);

	util::CopyOvlFileOptional(Shared, destination);

	if (SlopeStraight2 != SlopeStraight1 && !String::IsNullOrWhiteSpace(SlopeStraight2))
	{
		util::CopyOvlFile(SlopeStraight2, destination);
	}
}

void MQueue::CreateTextureOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	String^ texture = Texture;

	unsigned int recolor = 0;

	if (Recolor1) recolor |= RCT3Asset::RecolorOptions::FirstColor;
	if (Recolor2) recolor |= RCT3Asset::RecolorOptions::SecondColor;
	if (Recolor3) recolor |= RCT3Asset::RecolorOptions::ThirdColor;

	RCT3Asset::FtxImage ftxImg(log->Native());
	ftxImg.FromFile(util::std_string(texture));

	RCT3Asset::FlexiTextureFrame main(ftxImg);
	main.Recolorability(recolor);

	RCT3Asset::FlexiTexture ftx;
	ftx.Name(util::std_string(Path::GetFileNameWithoutExtension(texture)));
	ftx.MakeStillImage(main);

	RCT3Asset::FlexiTextureCollection ftxCol;
	ftxCol.Add(ftx);

	ftxCol.AddTo(ovl);

	ovl.Save(util::std_string(path));
}

void MQueue::CreateIconOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	RCT3Asset::TextureStyle txs = RCT3Asset::TextureStyle::GUIIcon;
	txs.AddTo(ovl);

	RCT3Asset::TexImage imgIcon(log->Native());
	imgIcon.FromFile(util::std_string(Icon));

	RCT3Asset::TextureMip mainMip(imgIcon);

	RCT3Asset::Texture tex;
	tex.Name(util::std_string(Path::GetFileNameWithoutExtension(Icon)));
	tex.TxsStyle = txs;
	tex.Mips.push_back(mainMip);

	RCT3Asset::GuiSkinItem gsiIcon;
	gsiIcon.Name(util::std_string(Name + "_Icon"));

	RCT3Asset::IconPosition pos;

	pos.Top = 0;
	pos.Left = 0;
	pos.Right = 40;
	pos.Bottom = 40;

	gsiIcon.Position = pos;
	gsiIcon.Texture = tex;

	RCT3Asset::FlicManager flic;
	flic.Add(tex);
	flic.CreateAndAssign(ovl);

	RCT3Asset::GuiSkinItemCollection gsiCol;
	gsiCol.Add(gsiIcon);
	gsiCol.AddTo(ovl);

	RCT3Asset::TextureCollection texCol;
	texCol.Add(tex);
	texCol.AddTo(ovl);

	ovl.Save(util::std_string(path));
}

void MQueue::CreateStubOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	std::string stdname = util::std_string(Name);

	ovl.AddFileReference(stdname + "_Icon");

	RCT3Asset::TextString text;
	text.Name(util::std_string(Name + "_Text"));
	text.Text(util::std_wstring(IngameName));

	RCT3Asset::TextStringCollection txtCol;
	txtCol.Add(text);

	RCT3Asset::Queue qtd;

	qtd.Name(util::std_string(Name));
	qtd.GsiIcon = util::std_string(Name + "_Icon:gsi");
	qtd.Text = text;
	qtd.FtxTexture = util::std_string(Path::GetFileNameWithoutExtension(Texture)) + ":ftx";

	qtd.Straight = util::GetOvlName_std(Straight);
	qtd.TurnL = util::GetOvlName_std(TurnL);
	qtd.TurnR = util::GetOvlName_std(TurnR);
	qtd.SlopeUp = util::GetOvlName_std(SlopeUp);
	qtd.SlopeDown = util::GetOvlName_std(SlopeDown);
	qtd.SlopeStraight1 = util::GetOvlName_std(SlopeStraight1);

	if (SlopeStraight2 != SlopeStraight1 && !String::IsNullOrWhiteSpace(SlopeStraight2))
		qtd.SlopeStraight2 = util::GetOvlName_std(SlopeStraight2);
	else
		qtd.SlopeStraight2 = qtd.SlopeStraight1;

	RCT3Asset::QueueCollection qtdCol;
	qtdCol.Add(qtd);

	RCT3Asset::SceneryItemCollection sidCol;

	std::string ovlPath = "Queue\\" + stdname + "\\";

	RCT3Asset::SceneryItem sid;
	sid.SceneryItemType = RCT3Asset::SIDType::Path;
	sid.Size.X = 4.0f;
	sid.Size.Y = 3.0f;
	sid.Size.Z = 4.0f;

	DOQUEUESECTION(Straight);
	DOQUEUESECTION(TurnL);
	DOQUEUESECTION(TurnR);
	DOQUEUESECTION(SlopeUp);
	DOQUEUESECTION(SlopeDown);
	DOQUEUESECTION(SlopeStraight1);

	if (SlopeStraight2 != SlopeStraight1 && !String::IsNullOrWhiteSpace(SlopeStraight2))
	{
		DOQUEUESECTION(SlopeStraight2);
	}

	qtdCol.AddTo(ovl);
	sidCol.AddTo(ovl);
	txtCol.AddTo(ovl);

	ovl.Save(marshal_as<std::string>(path));
}

void MQueue::CreateBlankOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	ovl.Save(util::std_string(path));
}

#pragma endregion