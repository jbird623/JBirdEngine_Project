using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

	namespace ColorLibrary {

		[CreateAssetMenu]
		public class JBirdColorPalette : ScriptableObject {

			public List<ColorHelper.ColorHSVRGB> colors;

			public JBirdColorPalette () {
				colors = new List<ColorHelper.ColorHSVRGB>();
				for (int i = 0; i < 5; i++) {
					colors.Add(new ColorHelper.ColorHSVRGB());
					colors[i].rgb = Color.blue;
					colors[i].hsv = colors[i].rgb.ToHSV();
				}
			}

		}

	}

}
