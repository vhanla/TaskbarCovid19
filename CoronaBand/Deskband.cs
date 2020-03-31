using CSDeskBand;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SimpleInjector;
using System;
using CSDeskBand.ContextMenu;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System.Text.RegularExpressions;

namespace CoronaBand
{
    [ComVisible(true)]
    [Guid("9BDAB284-01E7-4B4F-B0A5-7CD016EE83EF")]
    [CSDeskBandRegistration(Name = "Coronavirus")]
    public class Deskband : CSDeskBandWin
    {
        private UserControl1 _mainControl;
        private Container _container;
        private frmStats _frmStats;

        private Boolean showStats = false;
        public Deskband()
        {
            Options.MinHorizontalSize = new Size(110, 30);
            Options.ContextMenuItems = ContextMenuItems;
            InitDependencies();
            
            _mainControl = _container.GetInstance<UserControl1>();
            _frmStats = _container.GetInstance<frmStats>();

            _mainControl.MouseEnter += showPopup;
            _mainControl.MouseLeave += hidePopup;
            _mainControl.label1.MouseMove += moveMouse;
        }

        protected override Control Control =>  _mainControl;

        protected override void DeskbandOnClosed()
        {
            base.DeskbandOnClosed();
            _mainControl.Hide();
            _mainControl = null;
        }

        private List<DeskBandMenuItem> ContextMenuItems
        {
            get
            {
                var action = new DeskBandMenuAction("About CoronaBand");                
                var separator = new DeskBandMenuSeparator();
                var update = new DeskBandMenuAction("Clear");
                var stats = new DeskBandMenuAction("Show Stats");
                stats.Checked = false;
                //Countries by continent
                // Africa
                var itemAF = new DeskBandMenuAction("Afghanistan (AF)"); itemAF.Clicked += itemClicked;
                var itemAL = new DeskBandMenuAction("Albania (AL)"); itemAL.Clicked += itemClicked;
                var itemDZ = new DeskBandMenuAction("Algeria (DZ)"); itemDZ.Clicked += itemClicked;
                var itemAD = new DeskBandMenuAction("Andorra (AD)"); itemAD.Clicked += itemClicked;
                var itemAO = new DeskBandMenuAction("Angola (AO)"); itemAO.Clicked += itemClicked;
                var itemAG = new DeskBandMenuAction("Antigua and Barbuda (AG)"); itemAG.Clicked += itemClicked;
                var itemAR = new DeskBandMenuAction("Argentina (AR)"); itemAR.Clicked += itemClicked;
                var itemAM = new DeskBandMenuAction("Armenia (AM)"); itemAM.Clicked += itemClicked;
                // needs to include other regions
                var itemAU = new DeskBandMenuAction("Australia (AU)"); itemAU.Clicked += itemClicked;
                var itemAT = new DeskBandMenuAction("Austria (AT)"); itemAT.Clicked += itemClicked;
                var itemAZ = new DeskBandMenuAction("Azerbaijan (AZ)"); itemAZ.Clicked += itemClicked;
                var itemBS = new DeskBandMenuAction("Bahamas (BS)"); itemBS.Clicked += itemClicked;
                var itemBH = new DeskBandMenuAction("Bahrain (BH)"); itemBH.Clicked += itemClicked;
                var itemBD = new DeskBandMenuAction("Bangladesh (BD)"); itemBD.Clicked += itemClicked;
                var itemBB = new DeskBandMenuAction("Barbados (BB)"); itemBB.Clicked += itemClicked;
                var itemBY = new DeskBandMenuAction("Belarus (BY)"); itemBY.Clicked += itemClicked;
                var itemBE = new DeskBandMenuAction("Belgium (BE)"); itemBE.Clicked += itemClicked;
                var itemBJ = new DeskBandMenuAction("Benin (BJ)"); itemBJ.Clicked += itemClicked;
                var itemBT = new DeskBandMenuAction("Bhutan (BT)"); itemBT.Clicked += itemClicked;
                var itemBO = new DeskBandMenuAction("Bolivia (BO)"); itemBO.Clicked += itemClicked;
                var itemBA = new DeskBandMenuAction("Bosnia and Herzegovina (BA)"); itemBA.Clicked += itemClicked;
                var itemBR = new DeskBandMenuAction("Brazil (BR)"); itemBR.Clicked += itemClicked;
                var itemBN = new DeskBandMenuAction("Brunei (BN)"); itemBN.Clicked += itemClicked;
                var itemBG = new DeskBandMenuAction("Bulgaria (BG)"); itemBG.Clicked += itemClicked;
                var itemBF = new DeskBandMenuAction("Burkina Faso (BF)"); itemBF.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemBI = new DeskBandMenuAction("Burundi (BI)"); itemBI.Clicked += itemClicked;
                var itemCV = new DeskBandMenuAction("Cabo Verde (CV)"); itemCV.Clicked += itemClicked;
                var itemKH = new DeskBandMenuAction("Cambodia (KH)"); itemKH.Clicked += itemClicked;
                var itemCM = new DeskBandMenuAction("Cameroon (CM)"); itemCM.Clicked += itemClicked;
                // needs other regions too
                var itemCA = new DeskBandMenuAction("Canada (CA)"); itemCA.Clicked += itemClicked;
                var itemCF = new DeskBandMenuAction("Central African Republic (CF)"); itemCF.Clicked += itemClicked;
                var itemTD = new DeskBandMenuAction("Chad (TD)"); itemTD.Clicked += itemClicked;
                var itemCL = new DeskBandMenuAction("Chile (CL)"); itemCL.Clicked += itemClicked;
                // needs other regions too
                var itemCN = new DeskBandMenuAction("China (CN)"); itemCN.Clicked += itemClicked;
                var itemCO = new DeskBandMenuAction("Colombia (CO)"); itemCO.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                var itemKM = new DeskBandMenuAction("Comoros (KM)"); itemKM.Clicked += itemClicked;
                var itemCG = new DeskBandMenuAction("Congo (Brazzaville) (CG)"); itemCG.Clicked += itemClicked;
                var itemCD = new DeskBandMenuAction("Congo (Kinshasa) (CD)"); itemCD.Clicked += itemClicked;
                var itemCR = new DeskBandMenuAction("Costa Rica (CR)"); itemCR.Clicked += itemClicked;
                var itemCI = new DeskBandMenuAction("Cote d'Ivoire (CI)"); itemCI.Clicked += itemClicked;
                var itemHR = new DeskBandMenuAction("Croatia (HR)"); itemHR.Clicked += itemClicked;
                //var itemXX = new DeskBandMenuAction("Diamond Princess (XX)"); itemXX.Clicked += itemClicked;
                var itemCU = new DeskBandMenuAction("Cuba (CU)"); itemCU.Clicked += itemClicked;
                var itemCY = new DeskBandMenuAction("Cyprus (CY)"); itemCY.Clicked += itemClicked;
                var itemCZ = new DeskBandMenuAction("Czechia (CZ)"); itemCZ.Clicked += itemClicked;
                // needs other regions too
                var itemDK = new DeskBandMenuAction("Denmark (DK)"); itemDK.Clicked += itemClicked;
                var itemDJ = new DeskBandMenuAction("Djibouti (DJ)"); itemDJ.Clicked += itemClicked;
                var itemDO = new DeskBandMenuAction("Dominican Republic (DO)"); itemDO.Clicked += itemClicked;
                var itemEC = new DeskBandMenuAction("Ecuador (EC)"); itemEC.Clicked += itemClicked;
                var itemEG = new DeskBandMenuAction("Egypt (EG)"); itemEG.Clicked += itemClicked;
                var itemSV = new DeskBandMenuAction("El Salvador (SV)"); itemSV.Clicked += itemClicked;
                var itemGQ = new DeskBandMenuAction("Equatorial Guinea (GQ)"); itemGQ.Clicked += itemClicked;
                var itemER = new DeskBandMenuAction("Eritrea (ER)"); itemER.Clicked += itemClicked;
                var itemEE = new DeskBandMenuAction("Estonia (EE)"); itemEE.Clicked += itemClicked;
                var itemSZ = new DeskBandMenuAction("Eswatini (SZ)"); itemSZ.Clicked += itemClicked;
                var itemET = new DeskBandMenuAction("Ethiopia (ET)"); itemET.Clicked += itemClicked;
                var itemFJ = new DeskBandMenuAction("Fiji (FJ)"); itemFJ.Clicked += itemClicked;
                var itemFI = new DeskBandMenuAction("Finland (FI)"); itemFI.Clicked += itemClicked;
                // needs other regions too
                var itemFR = new DeskBandMenuAction("France (FR)"); itemFR.Clicked += itemClicked;
                var itemGA = new DeskBandMenuAction("Gabon (GA)"); itemGA.Clicked += itemClicked;
                var itemGM = new DeskBandMenuAction("Gambia (GM)"); itemGM.Clicked += itemClicked;
                var itemGE = new DeskBandMenuAction("Georgia (GE)"); itemGE.Clicked += itemClicked;
                var itemDE = new DeskBandMenuAction("Germany (DE)"); itemDE.Clicked += itemClicked;
                var itemGH = new DeskBandMenuAction("Ghana (GH)"); itemGH.Clicked += itemClicked;
                var itemGR = new DeskBandMenuAction("Greece (GR)"); itemGR.Clicked += itemClicked;
                var itemGT = new DeskBandMenuAction("Guatemala (GT)"); itemGT.Clicked += itemClicked;
                var itemGN = new DeskBandMenuAction("Guinea (GN)"); itemGN.Clicked += itemClicked;
                var itemGY = new DeskBandMenuAction("Guyana (GY)"); itemGY.Clicked += itemClicked;
                var itemHT = new DeskBandMenuAction("Haiti (HT)"); itemHT.Clicked += itemClicked;
                var itemVA = new DeskBandMenuAction("Holy See (VA)"); itemVA.Clicked += itemClicked;
                var itemHN = new DeskBandMenuAction("Honduras (HN)"); itemHN.Clicked += itemClicked;
                var itemHU = new DeskBandMenuAction("Hungary (HU)"); itemHU.Clicked += itemClicked;
                var itemIS = new DeskBandMenuAction("Iceland (IS)"); itemIS.Clicked += itemClicked;
                var itemIN = new DeskBandMenuAction("India (IN)"); itemIN.Clicked += itemClicked;
                var itemID = new DeskBandMenuAction("Indonesia (ID)"); itemID.Clicked += itemClicked;
                var itemIR = new DeskBandMenuAction("Iran (IR)"); itemIR.Clicked += itemClicked;
                var itemIQ = new DeskBandMenuAction("Iraq (IQ)"); itemIQ.Clicked += itemClicked;
                var itemIE = new DeskBandMenuAction("Ireland (IE)"); itemIE.Clicked += itemClicked;
                var itemIL = new DeskBandMenuAction("Israel (IL)"); itemIL.Clicked += itemClicked;
                var itemIT = new DeskBandMenuAction("Italy (IT)"); itemIT.Clicked += itemClicked;
                var itemJM = new DeskBandMenuAction("Jamaica (JM)"); itemJM.Clicked += itemClicked;
                var itemJP = new DeskBandMenuAction("Japan (JP)"); itemJP.Clicked += itemClicked;
                var itemJO = new DeskBandMenuAction("Jordan (JO)"); itemJO.Clicked += itemClicked;
                var itemKZ = new DeskBandMenuAction("Kazakhstan (KZ)"); itemKZ.Clicked += itemClicked;
                var itemKE = new DeskBandMenuAction("Kenya (KE)"); itemKE.Clicked += itemClicked;
                var itemKR = new DeskBandMenuAction("Korea, South (KR)"); itemKR.Clicked += itemClicked;
                var itemKW = new DeskBandMenuAction("Kuwait (KW)"); itemKW.Clicked += itemClicked;
                var itemKG = new DeskBandMenuAction("Kyrgyzstan (KG)"); itemKG.Clicked += itemClicked;
                var itemLV = new DeskBandMenuAction("Latvia (LV)"); itemLV.Clicked += itemClicked;
                var itemLB = new DeskBandMenuAction("Lebanon (LB)"); itemLB.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemLS = new DeskBandMenuAction("Lesotho (LS)"); itemLS.Clicked += itemClicked;
                var itemLR = new DeskBandMenuAction("Liberia (LR)"); itemLR.Clicked += itemClicked;
                var itemLI = new DeskBandMenuAction("Liechtenstein (LI)"); itemLI.Clicked += itemClicked;
                var itemLT = new DeskBandMenuAction("Lithuania (LT)"); itemLT.Clicked += itemClicked;
                var itemLU = new DeskBandMenuAction("Luxembourg (LU)"); itemLU.Clicked += itemClicked;
                var itemMG = new DeskBandMenuAction("Madagascar (MG)"); itemMG.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemMW = new DeskBandMenuAction("Malawi (MW)"); itemMW.Clicked += itemClicked;
                var itemMY = new DeskBandMenuAction("Malaysia (MY)"); itemMY.Clicked += itemClicked;
                var itemMV = new DeskBandMenuAction("Maldives (MV)"); itemMV.Clicked += itemClicked;
                var itemMT = new DeskBandMenuAction("Malta (MT)"); itemMT.Clicked += itemClicked;
                var itemMR = new DeskBandMenuAction("Mauritania (MR)"); itemMR.Clicked += itemClicked;
                var itemMU = new DeskBandMenuAction("Mauritius (MU)"); itemMU.Clicked += itemClicked;
                var itemMX = new DeskBandMenuAction("Mexico (MX)"); itemMX.Clicked += itemClicked;
                var itemMD = new DeskBandMenuAction("Moldova (MD)"); itemMD.Clicked += itemClicked;
                var itemMC = new DeskBandMenuAction("Monaco (MC)"); itemMC.Clicked += itemClicked;
                var itemMN = new DeskBandMenuAction("Mongolia (MN)"); itemMN.Clicked += itemClicked;
                var itemME = new DeskBandMenuAction("Montenegro (ME)"); itemME.Clicked += itemClicked;
                var itemMA = new DeskBandMenuAction("Morocco (MA)"); itemMA.Clicked += itemClicked;
                var itemNA = new DeskBandMenuAction("Namibia (NA)"); itemNA.Clicked += itemClicked;
                var itemNP = new DeskBandMenuAction("Nepal (NP)"); itemNP.Clicked += itemClicked;
                // needs other regions too
                var itemNL = new DeskBandMenuAction("Netherlands (NL)"); itemNL.Clicked += itemClicked;
                var itemNZ = new DeskBandMenuAction("New Zealand (NZ)"); itemNZ.Clicked += itemClicked;
                var itemNI = new DeskBandMenuAction("Nicaragua (NI)"); itemNI.Clicked += itemClicked;
                var itemNE = new DeskBandMenuAction("Niger (NE)"); itemNE.Clicked += itemClicked;
                var itemNG = new DeskBandMenuAction("Nigeria (NG)"); itemNG.Clicked += itemClicked;
                var itemMK = new DeskBandMenuAction("North Macedonia (MK)"); itemMK.Clicked += itemClicked;
                var itemNO = new DeskBandMenuAction("Norway (NO)"); itemNO.Clicked += itemClicked;
                var itemOM = new DeskBandMenuAction("Oman (OM)"); itemOM.Clicked += itemClicked;
                var itemPK = new DeskBandMenuAction("Pakistan (PK)"); itemPK.Clicked += itemClicked;
                var itemPA = new DeskBandMenuAction("Panama (PA)"); itemPA.Clicked += itemClicked;
                var itemPG = new DeskBandMenuAction("Papua New Guinea (PG)"); itemPG.Clicked += itemClicked;
                var itemPY = new DeskBandMenuAction("Paraguay (PY)"); itemPY.Clicked += itemClicked;
                var itemPE = new DeskBandMenuAction("Peru (PE)"); itemPE.Clicked += itemClicked;
                var itemPH = new DeskBandMenuAction("Philippines (PH)"); itemPH.Clicked += itemClicked;
                var itemPL = new DeskBandMenuAction("Poland (PL)"); itemPL.Clicked += itemClicked;
                var itemPT = new DeskBandMenuAction("Portugal (PT)"); itemPT.Clicked += itemClicked;
                var itemQA = new DeskBandMenuAction("Qatar (QA)"); itemQA.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemRE = new DeskBandMenuAction("Reunion (RE)"); itemRE.Clicked += itemClicked;
                var itemRO = new DeskBandMenuAction("Romania (RO)"); itemRO.Clicked += itemClicked;
                var itemRU = new DeskBandMenuAction("Russia (RU)"); itemRU.Clicked += itemClicked;
                var itemRW = new DeskBandMenuAction("Rwanda (RW)"); itemRW.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemSH = new DeskBandMenuAction("Saint Helena (SH)"); itemSH.Clicked += itemClicked;
                var itemLC = new DeskBandMenuAction("Saint Lucia (LC)"); itemLC.Clicked += itemClicked;
                var itemVC = new DeskBandMenuAction("Saint Vincent and the Grenadines (VC)"); itemVC.Clicked += itemClicked;
                var itemSM = new DeskBandMenuAction("San Marino (SM)"); itemSM.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemST = new DeskBandMenuAction("Sao Tome and Principe (ST)"); itemST.Clicked += itemClicked;
                var itemSA = new DeskBandMenuAction("Saudi Arabia (SA)"); itemSA.Clicked += itemClicked;
                var itemSN = new DeskBandMenuAction("Senegal (SN)"); itemSN.Clicked += itemClicked;
                var itemRS = new DeskBandMenuAction("Serbia (RS)"); itemRS.Clicked += itemClicked;
                var itemSC = new DeskBandMenuAction("Seychelles (SC)"); itemSC.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemSL = new DeskBandMenuAction("Sierra Leone (SL)"); itemSL.Clicked += itemClicked;
                var itemSG = new DeskBandMenuAction("Singapore (SG)"); itemSG.Clicked += itemClicked;
                var itemSK = new DeskBandMenuAction("Slovakia (SK)"); itemSK.Clicked += itemClicked;
                var itemSI = new DeskBandMenuAction("Slovenia (SI)"); itemSI.Clicked += itemClicked;
                var itemSO = new DeskBandMenuAction("Somalia (SO)"); itemSO.Clicked += itemClicked;
                var itemZA = new DeskBandMenuAction("South Africa (ZA)"); itemZA.Clicked += itemClicked;
                var itemSS = new DeskBandMenuAction("South Sudan (SS)"); itemSS.Clicked += itemClicked;
                var itemES = new DeskBandMenuAction("Spain (ES)"); itemES.Clicked += itemClicked;
                var itemLK = new DeskBandMenuAction("Sri Lanka (LK)"); itemLK.Clicked += itemClicked;
                var itemSD = new DeskBandMenuAction("Sudan (SD)"); itemSD.Clicked += itemClicked;
                var itemSR = new DeskBandMenuAction("Suriname (SR)"); itemSR.Clicked += itemClicked;
                var itemSE = new DeskBandMenuAction("Sweden (SE)"); itemSE.Clicked += itemClicked;
                var itemCH = new DeskBandMenuAction("Switzerland (CH)"); itemCH.Clicked += itemClicked;
                var itemTW = new DeskBandMenuAction("Taiwan* (TW)"); itemTW.Clicked += itemClicked;
                var itemTZ = new DeskBandMenuAction("Tanzania (TZ)"); itemTZ.Clicked += itemClicked;
                var itemTH = new DeskBandMenuAction("Thailand (TH)"); itemTH.Clicked += itemClicked;
                var itemTG = new DeskBandMenuAction("Togo (TG)"); itemTG.Clicked += itemClicked;
                var itemTT = new DeskBandMenuAction("Trinidad and Tobago (TT)"); itemTT.Clicked += itemClicked;
                var itemTN = new DeskBandMenuAction("Tunisia (TN)"); itemTN.Clicked += itemClicked;
                var itemTR = new DeskBandMenuAction("Turkey (TR)"); itemTR.Clicked += itemClicked;
                var itemUG = new DeskBandMenuAction("Uganda (UG)"); itemUG.Clicked += itemClicked;
                var itemUA = new DeskBandMenuAction("Ukraine (UA)"); itemUA.Clicked += itemClicked;
                var itemAE = new DeskBandMenuAction("United Arab Emirates (AE)"); itemAE.Clicked += itemClicked;
                // needs other regions too
                var itemGB = new DeskBandMenuAction("United Kingdom (GB)"); itemGB.Clicked += itemClicked;
                var itemUY = new DeskBandMenuAction("Uruguay (UY)"); itemUY.Clicked += itemClicked;
                var itemUZ = new DeskBandMenuAction("Uzbekistan (UZ)"); itemUZ.Clicked += itemClicked;
                var itemVE = new DeskBandMenuAction("Venezuela (VE)"); itemVE.Clicked += itemClicked;
                var itemVN = new DeskBandMenuAction("Vietnam (VN)"); itemVN.Clicked += itemClicked;
                var itemZM = new DeskBandMenuAction("Zambia (ZM)"); itemZM.Clicked += itemClicked;
                var itemZW = new DeskBandMenuAction("Zimbabwe (ZW)"); itemZW.Clicked += itemClicked;
                var itemDM = new DeskBandMenuAction("Dominica (DM)"); itemDM.Clicked += itemClicked;
                var itemGD = new DeskBandMenuAction("Grenada (GD)"); itemGD.Clicked += itemClicked;
                var itemMZ = new DeskBandMenuAction("Mozambique (MZ)"); itemMZ.Clicked += itemClicked;
                var itemSY = new DeskBandMenuAction("Syria (SY)"); itemSY.Clicked += itemClicked;
                var itemTL = new DeskBandMenuAction("Timor-Leste (TL)"); itemTL.Clicked += itemClicked;
                var itemBZ = new DeskBandMenuAction("Belize (BZ)"); itemBZ.Clicked += itemClicked;
                var itemLA = new DeskBandMenuAction("Laos (LA)"); itemLA.Clicked += itemClicked;
                var itemLY = new DeskBandMenuAction("Libya (LY)"); itemLY.Clicked += itemClicked;
                var itemPS = new DeskBandMenuAction("West Bank and Gaza (PS)"); itemPS.Clicked += itemClicked;
                // was not listed in JSON maybe belongs to another country, results uselessly ignore them
                //var itemEH = new DeskBandMenuAction("Western Sahara (EH)"); itemEH.Clicked += itemClicked;
                var itemGW = new DeskBandMenuAction("Guinea-Bissau (GW)"); itemGW.Clicked += itemClicked;
                var itemML = new DeskBandMenuAction("Mali (ML)"); itemML.Clicked += itemClicked;
                var itemKN = new DeskBandMenuAction("Saint Kitts and Nevis (KN)"); itemKN.Clicked += itemClicked;
                var itemXK = new DeskBandMenuAction("Kosovo (XK)"); itemXK.Clicked += itemClicked;
                //var itemXX = new DeskBandMenuAction("Burma (XX)"); itemXX.Clicked += itemClicked;
                //var itemXX = new DeskBandMenuAction("MS Zaandam (XX)"); itemXX.Clicked += itemClicked;
                var itemBW = new DeskBandMenuAction("Botswana (BW)"); itemBW.Clicked += itemClicked;
                // ignored countries because they might belong to another
                /* SA/NA:
                 * AI-Anguila (UK)
                 * AW-Aruba (Netherlands)
                 * BM-Bermuda (UK)
                 * VG-British Virgin Islands (UK)
                 * KY-Cayman Islands (UK)
                 * CW-Curacao (Netherlands)
                 * FK-Falkland Islands (UK)
                 * GL-Greenland (Denmark)
                 * GP-Guadaloupe (France)
                 * MS-Monserrat (UK)
                 * PR-Puerto Rico (US)
                 * BL-Saint Barthélemy (France)
                 * MF-Saint Martin (France)
                 * PM-Saint Pierre and Miquelon (France)
                 * ASIA:
                 * HK-Hong Kong (China)
                 * MO-Macau
                 * MM-Myanmar [Burma]
                 * KP-North Korea
                 * TJ-Tajikistan
                 * TM-Turkmenistan
                 * YE-Yemen
                 */
                var africa = new DeskBandMenu("Africa")
                {
                    Items = { itemDZ, itemAO, itemBJ, itemBW, itemBF, /*itemBI,*/ itemCM, itemCV,itemCF, itemKM, itemCD, 
                              itemDJ, itemEG, itemGQ, itemER, itemET, itemGA, itemGM, itemGH, itemGN, itemGW, itemCI, 
                              itemKE, /*itemLS,*/ itemLR, itemLY, itemMG, /*itemMW,*/ itemML, itemMR, itemMU, itemMA, itemMZ, 
                              itemNA, itemNE, itemNG, itemCG, /*itemRE,*/ itemRW, /*itemSH, itemST,*/ itemSN, itemSC, /*itemSL,*/ 
                              itemSO, itemZA, itemSS, itemSD, itemSZ, itemTZ, itemTG, itemTN, itemUG, /*itemEH,*/ itemZM, itemZW}
                };
                // Asia

                var asia = new DeskBandMenu("Asia")
                {
                    Items = { itemAF, itemAM, itemAZ, itemBH, itemBD, itemBT, itemBN, itemKH, itemCN, itemGE, /*itemHK,*/ itemIN,
                              itemID, itemIR, itemIQ, itemIL, itemJP, itemJO, itemKZ, itemKW, itemKG, itemLA, itemLB, /*itemMO,*/
                              itemMY, itemMV, itemMN, /*itemMM,*/ itemNP, /*itemKP,*/ itemOM, itemPK, itemPH, itemQA, itemSA, itemSG,
                              itemKR, itemLK, itemSY, itemTW, /*itemTJ,*/ itemTH, itemTR, /*itemTM,*/ itemAE, itemUZ, itemVN/*, itemYE*/}
                };
                // Europe
                /*
                 * Not included
                 * FO-Faroe Islands
                 * GI-Gibraltar
                 * IM-Isle of Man
                 */
                var europe = new DeskBandMenu("Europe")
                {
                    Items = { itemAL, itemAD, itemAT, itemBY, itemBE, itemBA, itemBG, itemHR, itemCY, itemCZ,
                              itemDK, itemEE, /*itemFO,*/ itemFI, itemFR, itemDE, /*itemGI,*/ itemGR, itemHU, itemIS,
                              itemIE, /*itemIM,*/ itemIT, itemXK, itemLV, itemLI, itemLT, itemLU, itemMK, itemMT,
                              itemMD, itemMC, itemME, itemNL, itemNO, itemPL, itemPT, itemRO, itemRU, itemSM,
                              itemRS, itemSK, itemSI, itemES, itemSE, itemCH, itemUA, itemGB, itemVA}
                };
                // North America
                var itemUS = new DeskBandMenuAction("United States (US)"); itemUS.Clicked += itemClicked;
                var northamerica = new DeskBandMenu("North America")
                {
                    Items = { /*itemAI, itemAW,*/ itemBS, itemBB, itemBZ, /*itemBM, itemVG,*/ itemCA, /*itemKY,*/ itemCR, itemCU,
                        /*itemCW,*/ itemDM, itemDO, itemSV, /*itemFK, itemGL, itemGP,*/ itemGT, itemHT, itemHN, itemJM, itemMX,
                        /*itemMS,*/ itemNI, itemPA, /*itemPR, itemBL,*/ itemKN, itemLC, /*itemMF, itemPM,*/ itemVC, itemTT, itemUS}
                };
                // South America
                var southamerica = new DeskBandMenu("South America")
                {
                    Items = { itemAR, itemBO, itemBR, itemCL, itemCO, itemEC, itemGY, itemPY, itemPE, itemSR, itemUY, itemVE }
                };
                // Oceania
                /* Not included
                 * AS-American Samoa
                 * CK-Cook Islands
                 * PF-French Polynesia
                 * GU-Guam
                 * KI-Kiribati
                 * MH-Marshall Islands
                 * FM-Micronesia
                 * NR-Nauru
                 * NC-New Caledonia
                 * NU-Niue
                 * NF-Norfolk Island
                 * MP-Northern Mariana Islands
                 * PW-Palau
                 * PN-Pitcairn Islands
                 * WS-Samoa
                 * SB-Somolon Islands
                 * TK-Tokelau
                 * TV-Tuvalu
                 * VU-Vanuatu
                 */
                var oceania = new DeskBandMenu("Oceania/Australia")
                {
                    Items = { /*itemAS,*/ itemAU, /*itemCK,*/ itemTL, itemFJ, /*itemPF, itemGU, itemKI, itemMH, itemFM, itemNR,
                              itemNC,*/ itemNZ, /*itemNU, itemNF, itemMP, itemPW,*/ itemPG/*, itemPN, itemWS, itemSB, itemTK,
                              itemTV, itemVU*/}
                };
                var submenu = new DeskBandMenu("Countries")
                {
                    //Items = { africa, asia, europe, northamerica, southamerica, oceania }
                };
                action.Clicked += dlgABout;
                update.Clicked += updateData;
                stats.Clicked += ShowStats;
                return new List<DeskBandMenuItem>() { action, separator, stats, update, separator, africa, asia, europe, northamerica, southamerica, oceania };
            }
        }
        private static void dlgABout(Object sender, EventArgs e)
        {
            MessageBox.Show("CoronaBand v1.0\n\nShows Coronavirus updated data in your taskbar");
        }
        private void updateData(Object sedner, EventArgs e)
        {
            _frmStats.cartesianChart1.Series.Clear();
            _frmStats.cartesianChart1.Series.Add(new LineSeries { Title = "PE", Values = new ChartValues<int> { } });
            _frmStats.UpdateData("PE", "Peru (PE)");
            _mainControl.UpdateData("PE", "Peru (PE)");
        }
        private void ShowStats(Object sender, EventArgs e)
        {
            var menuItem = (DeskBandMenuAction)sender;
            menuItem.Checked = !menuItem.Checked;
            showStats = menuItem.Checked;
            if (!menuItem.Checked)
            {
                _frmStats.Hide();
            }
        }
        private void itemClicked(Object sedner, EventArgs e)
        {
            var menuItem = (DeskBandMenuAction)sedner;
            var locale = Regex.Match(menuItem.Text, @"\(([^)]*)\)").Groups[1].Value;

            _frmStats.UpdateData(locale, menuItem.Text, true);
            _mainControl.UpdateData(locale, menuItem.Text);
        }
        private void InitDependencies()
        {
            try
            {
                _mainControl = new UserControl1();
                _frmStats = new frmStats();
                _container = new Container();
                _container.RegisterInstance(Options);
                _container.RegisterInstance(TaskbarInfo);

                _container.Verify();

            } catch (Exception)
            {
                throw;
            }
        }

        private void showPopup(Object sender, EventArgs e)
        {
            _frmStats.Left = (Cursor.Position.X) - (_frmStats.Width / 2);
            _frmStats.Top = Cursor.Position.Y - 32 - _frmStats.Height;
            if (showStats)
                _frmStats.Show();
        }
        private void hidePopup(Object sender, EventArgs e)
        {
            _frmStats.Hide();
        }

        private void moveMouse(Object sender, EventArgs e)
        {
            if (!_frmStats.Visible)
            {
                _frmStats.Left = (Cursor.Position.X) - (_frmStats.Width / 2);
                _frmStats.Top = Cursor.Position.Y - 32 - _frmStats.Height;

                if(showStats)
                    _frmStats.Show();
            }
        }
    }
}
