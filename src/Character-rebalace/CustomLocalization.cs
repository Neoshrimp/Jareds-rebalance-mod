﻿using I2.Loc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using UnityEngine;

namespace Character_rebalance
{
    class CustomLocalization

    {
		public static void InitLocalizationCSV()
		{
			// need to add EmbeddedResource tag to project config to work
			using (var sr = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream("Character_rebalance.Resources.localization.csv"), Encoding.UTF8))
			{
				string csv = sr.ReadToEnd();
				MainFile.Import_CSV("", csv, eSpreadsheetUpdateMode.Replace, ',');
			}
		}

		public static LanguageSourceData MainFile = new LanguageSourceData();
	}
}
