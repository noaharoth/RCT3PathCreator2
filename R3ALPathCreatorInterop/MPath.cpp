// PathHelper.cpp

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

#include "MPath.hpp"

using namespace R3ALInterop;

#pragma region Macros

// I hate macros, but it really helps with readability in this case

#define DO_SECTION(PTDSECTION) sid.Name(ptd.PTDSECTION[0]); \
	sid.TxtName = text; \
	sid.GsiIcon = ptd.GsiIcon; \
	sid.OvlPath = ovlPath + ptd.PTDSECTION[0]; \
	sid.Svds.push_back(ptd.PTDSECTION[0] + ":svd"); \
	sidCol.Add(sid); \
	sid.Svds.clear();

#define DO_OPTIONAL_SECTION(OPTSECTION) if (ptd.OPTSECTION[0].length()) { \
	sid.Name(ptd.OPTSECTION[0]); \
	sid.TxtName = text; \
	sid.GsiIcon = ptd.GsiIcon; \
	sid.OvlPath = ovlPath + ptd.OPTSECTION[0]; \
	sid.Svds.push_back(ptd.OPTSECTION[0] + ":svd"); \
	sidCol.Add(sid); \
	sid.Svds.clear(); } 

#pragma endregion

#pragma region MPathSection

MPathSection::MPathSection(String^ fileName)
{
	Section = fileName;
}

RCT3Asset::PathSection MPathSection::Convert()
{
	RCT3Asset::PathSection section;

	if (!String::IsNullOrWhiteSpace(Section))
		section = util::GetOvlName_std(Section);
	else
		section = "";

	return section;
}

#pragma endregion

#pragma region MPath

MPath::MPath()
{
	Name = "";
	IngameName = "";
	Icon = "";
	TextureA = "";
	TextureB = "";
	Shared = "";
	Flat = MPathSection("");
	StraightA = MPathSection("");
	StraightB = MPathSection("");
	CornerA = MPathSection("");
	CornerB = MPathSection("");
	CornerC = MPathSection("");
	CornerD = MPathSection("");
	TurnU = MPathSection("");
	TurnLA = MPathSection("");
	TurnLB = MPathSection("");
	TurnTA = MPathSection("");
	TurnTB = MPathSection("");
	TurnTC = MPathSection("");
	TurnX = MPathSection("");
	Slope = MPathSection("");
	SlopeStraight = MPathSection("");
	SlopeStraightL = MPathSection("");
	SlopeStraightR = MPathSection("");
	SlopeMid = MPathSection("");
	UnderwaterSupport = false;
	IsExtended = false;
	Unknown01 = 0;
	Unknown02 = 1;
	FlatFC = MPathSection("");
	SlopeFC = MPathSection("");
	SlopeBC = MPathSection("");
	SlopeTC = MPathSection("");
	SlopeStraightFC = MPathSection("");
	SlopeStraightBC = MPathSection("");
	SlopeStraightTC = MPathSection("");
	SlopeStraightLFC = MPathSection("");
	SlopeStraightLBC = MPathSection("");
	SlopeStraightLTC = MPathSection("");
	SlopeStraightRFC = MPathSection("");
	SlopeStraightRBC = MPathSection("");
	SlopeStraightRTC = MPathSection("");
	SlopeMidFC = MPathSection("");
	SlopeMidBC = MPathSection("");
	SlopeMidTC = MPathSection("");
	Paving = MPathSection("");
}

void MPath::CopyFilesTo(String^ destination)
{
	util::CopyOvlFile(Flat.Section, destination);
	util::CopyOvlFile(StraightA.Section, destination);
	util::CopyOvlFile(StraightB.Section, destination);
	util::CopyOvlFile(CornerA.Section, destination);
	util::CopyOvlFile(CornerB.Section, destination);
	util::CopyOvlFile(CornerC.Section, destination);
	util::CopyOvlFile(CornerD.Section, destination);
	util::CopyOvlFile(TurnU.Section, destination);
	util::CopyOvlFile(TurnLA.Section, destination);
	util::CopyOvlFile(TurnLB.Section, destination);
	util::CopyOvlFile(TurnTA.Section, destination);
	util::CopyOvlFile(TurnTB.Section, destination);
	util::CopyOvlFile(TurnTC.Section, destination);
	util::CopyOvlFile(TurnX.Section, destination);
	util::CopyOvlFile(Slope.Section, destination);
	util::CopyOvlFile(SlopeStraight.Section, destination);
	util::CopyOvlFile(SlopeStraightL.Section, destination);
	util::CopyOvlFile(SlopeStraightR.Section, destination);
	util::CopyOvlFile(SlopeMid.Section, destination);

	if (IsExtended)
	{
		util::CopyOvlFileOptional(FlatFC.Section, destination);
		util::CopyOvlFileOptional(SlopeFC.Section, destination);
		util::CopyOvlFileOptional(SlopeBC.Section, destination);
		util::CopyOvlFileOptional(SlopeTC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightFC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightBC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightTC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightLFC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightLBC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightLTC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightRFC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightRBC.Section, destination);
		util::CopyOvlFileOptional(SlopeStraightRTC.Section, destination);
		util::CopyOvlFileOptional(SlopeMidFC.Section, destination);
		util::CopyOvlFileOptional(SlopeMidBC.Section, destination);
		util::CopyOvlFileOptional(SlopeMidTC.Section, destination);
		util::CopyOvlFileOptional(Paving.Section, destination);
	}

	util::CopyOvlFileOptional(Shared, destination);
}

void MPath::CreateTextureOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	RCT3Asset::TextureStyle txs = RCT3Asset::TextureStyle::PathGround;

	txs.AddTo(ovl);

	RCT3Asset::TexImage imgA(log->Native());
	imgA.FromFile(util::std_string(TextureA));

	RCT3Asset::TextureMip mainAMip(imgA);

	RCT3Asset::Texture mainA;
	mainA.Name(util::std_string(Path::GetFileNameWithoutExtension(TextureA)));
	mainA.TxsStyle = txs;
	mainA.Mips.push_back(mainAMip);

	RCT3Asset::TexImage imgB(log->Native());
	imgB.FromFile(util::std_string(TextureB));

	RCT3Asset::TextureMip mainBMip(imgB);

	RCT3Asset::Texture mainB;
	mainB.Name(util::std_string(Path::GetFileNameWithoutExtension(TextureB)));
	mainB.TxsStyle = txs;
	mainB.Mips.push_back(mainBMip);

	// always create flic before textures
	RCT3Asset::FlicManager flic;
	flic.Add(mainA);
	flic.Add(mainB);
	flic.CreateAndAssign(ovl);

	// now we can create textures
	RCT3Asset::TextureCollection texCol;
	texCol.Add(mainA);
	texCol.Add(mainB);
	texCol.AddTo(ovl);

	ovl.Save(util::std_string(path));
}

void MPath::CreateIconOVL(String^ path, MOutputLog^ log)
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

void MPath::CreateStubOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	std::string stdname = util::std_string(Name);

	ovl.AddFileReference(stdname + "_Icon");

	RCT3Asset::TextString text;
	text.Name(util::std_string(Name + "_Text"));
	text.Text(util::std_wstring(IngameName));

	RCT3Asset::TextStringCollection txtCol;
	txtCol.Add(text);

	RCT3Asset::Path ptd;

	ptd.Name(util::std_string(Name));
	ptd.GsiIcon = util::std_string(Name + "_Icon:gsi");
	ptd.Text = text;

	ptd.Flags = RCT3Asset::PathFlags::Default;

	if (UnderwaterSupport)
		ptd.Flags |= RCT3Asset::PathFlags::Underwater;

	ptd.TextureA = util::std_string(Path::GetFileNameWithoutExtension(TextureA));
	ptd.TextureB = util::std_string(Path::GetFileNameWithoutExtension(TextureB));

	ptd.Flat = Flat.Convert();
	ptd.StraightA = StraightA.Convert();
	ptd.StraightB = StraightB.Convert();
	ptd.CornerA = CornerA.Convert();
	ptd.CornerB = CornerB.Convert();
	ptd.CornerC = CornerC.Convert();
	ptd.CornerD = CornerD.Convert();
	ptd.TurnU = TurnU.Convert();
	ptd.TurnLA = TurnLA.Convert();
	ptd.TurnLB = TurnLB.Convert();
	ptd.TurnTA = TurnTA.Convert();
	ptd.TurnTB = TurnTB.Convert();
	ptd.TurnTC = TurnTC.Convert();
	ptd.TurnX = TurnX.Convert();
	ptd.Slope = Slope.Convert();
	ptd.SlopeStraight = SlopeStraight.Convert();
	ptd.SlopeStraightLeft = SlopeStraightL.Convert();
	ptd.SlopeStraightRight = SlopeStraightR.Convert();
	ptd.SlopeMid = SlopeMid.Convert();

	if (IsExtended)
	{
		ptd.Flags |= RCT3Asset::PathFlags::Extended;
		ptd.Unknown01 = Unknown01;
		ptd.Unknown02 = Unknown02;
		ptd.FlatFC = FlatFC.Convert();
		ptd.SlopeFC = SlopeFC.Convert();
		ptd.SlopeBC = SlopeBC.Convert();
		ptd.SlopeTC = SlopeTC.Convert();
		ptd.SlopeStraightFC = SlopeStraightFC.Convert();
		ptd.SlopeStraightBC = SlopeStraightBC.Convert();
		ptd.SlopeStraightTC = SlopeStraightTC.Convert();
		ptd.SlopeStraightLeftFC = SlopeStraightLFC.Convert();
		ptd.SlopeStraightLeftBC = SlopeStraightLBC.Convert();
		ptd.SlopeStraightLeftTC = SlopeStraightLTC.Convert();
		ptd.SlopeStraightRightFC = SlopeStraightRFC.Convert();
		ptd.SlopeStraightRightBC = SlopeStraightRBC.Convert();
		ptd.SlopeStraightRightTC = SlopeStraightRTC.Convert();
		ptd.SlopeMidFC = SlopeMidFC.Convert();
		ptd.SlopeMidBC = SlopeMidBC.Convert();
		ptd.SlopeMidTC = SlopeMidTC.Convert();
		ptd.Paving = util::std_string(Paving.Section);
	}

	RCT3Asset::PathCollection ptdCol;
	ptdCol.Add(ptd);

	RCT3Asset::SceneryItemCollection sidCol;

	std::string ovlPath = "Path\\" + stdname + "\\";

	RCT3Asset::SceneryItem sid;
	sid.SceneryItemType = RCT3Asset::SIDType::Path;
	sid.Size.X = 4.0f;
	sid.Size.Y = 3.0f;
	sid.Size.Z = 4.0f;

	DO_SECTION(Flat);
	DO_SECTION(StraightA);
	DO_SECTION(StraightB);
	DO_SECTION(CornerA);
	DO_SECTION(CornerB);
	DO_SECTION(CornerC);
	DO_SECTION(CornerD);
	DO_SECTION(TurnU);
	DO_SECTION(TurnLA);
	DO_SECTION(TurnLB);
	DO_SECTION(TurnTA);
	DO_SECTION(TurnTB);
	DO_SECTION(TurnTC);
	DO_SECTION(TurnX);
	DO_SECTION(Slope);
	DO_SECTION(SlopeStraight);
	DO_SECTION(SlopeStraightLeft);
	DO_SECTION(SlopeStraightRight);
	DO_SECTION(SlopeMid);

	if (IsExtended)
	{
		DO_OPTIONAL_SECTION(FlatFC);
		DO_OPTIONAL_SECTION(SlopeFC);
		DO_OPTIONAL_SECTION(SlopeBC);
		DO_OPTIONAL_SECTION(SlopeTC);
		DO_OPTIONAL_SECTION(SlopeStraightFC);
		DO_OPTIONAL_SECTION(SlopeStraightBC);
		DO_OPTIONAL_SECTION(SlopeStraightTC);
		DO_OPTIONAL_SECTION(SlopeStraightLeftFC);
		DO_OPTIONAL_SECTION(SlopeStraightLeftBC);
		DO_OPTIONAL_SECTION(SlopeStraightLeftTC);
		DO_OPTIONAL_SECTION(SlopeStraightRightFC);
		DO_OPTIONAL_SECTION(SlopeStraightRightBC);
		DO_OPTIONAL_SECTION(SlopeStraightRightTC);
		DO_OPTIONAL_SECTION(SlopeMidFC);
		DO_OPTIONAL_SECTION(SlopeMidBC);
		DO_OPTIONAL_SECTION(SlopeMidTC);

		// Paving

		if (ptd.Paving.length())
		{
			sid.Name(ptd.Paving);
			sid.TxtName = text;
			sid.GsiIcon = ptd.GsiIcon;
			sid.OvlPath = ovlPath + ptd.Paving;
			sid.Svds.push_back(ptd.Paving + ":svd");
			sidCol.Add(sid);
			sid.Svds.clear();
		}
	}

	ptdCol.AddTo(ovl);
	sidCol.AddTo(ovl);
	txtCol.AddTo(ovl);

	ovl.Save(util::std_string(path));
}

void MPath::CreateBlankOVL(String^ path, MOutputLog^ log)
{
	RCT3Asset::OvlFile ovl(log->Native());

	ovl.Save(util::std_string(path));
}

#pragma endregion