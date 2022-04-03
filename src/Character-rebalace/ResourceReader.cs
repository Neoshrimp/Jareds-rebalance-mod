using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace Character_rebalance
{
    class ResourceReader

    {

        private static ResourceManager rm;

		public static ResourceManager ResourceManager
		{
			get
			{
				if (ResourceReader.rm == null)
				{
					//ResourceReader.rm = new ResourceManager("MoreItems.Properties.Resources", typeof(ResourceManager).Assembly);
					//typeof(ResourceManager).Assembly.GetManifestResourceStream("Character_Rebalance.Resources.localization.csv");
				}
				return ResourceReader.rm;
			}
		}
	}
}
