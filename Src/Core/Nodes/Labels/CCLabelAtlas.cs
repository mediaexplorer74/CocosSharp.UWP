using System.Diagnostics;
using System;

namespace CocosSharp
{
    public class CCLabelAtlas : CCAtlasNode, ICCTextContainer
    {
        protected char m_cMapStartChar;
        protected string m_sString = "";

        #region Properties


        public string Text
        {
            get { return m_sString; }
            set 
            {            
                // TODO: Check for null????
                int len = value.Length;
                if (len > TextureAtlas.TotalQuads)
                {
                    TextureAtlas.ResizeCapacity(len);
                }

                m_sString = value;

                UpdateAtlasValues();

                ContentSize = new CCSize(len * ItemWidth, ItemHeight);

                QuadsToDraw = len;

            }
        }

        #endregion Properties


        #region Constructors

        internal CCLabelAtlas()
        {
        }

        public CCLabelAtlas(string label, string fntFile) : this(label, new PlistDocument(CCFileUtils.GetFileData(fntFile)).Root as PlistDictionary)
        {
        }

        private CCLabelAtlas(string label, PlistDictionary fontPlistDict) 
            : this(label, fontPlistDict["textureFilename"].AsString, 
                (int)Math.Ceiling((double)fontPlistDict["itemWidth"].AsInt), 
                (int)Math.Ceiling((double)fontPlistDict["itemHeight"].AsInt),
                (char)fontPlistDict["firstChar"].AsInt)
        {
            Debug.Assert(fontPlistDict["version"].AsInt == 1, "Unsupported version. Upgrade cocos2d version");
        }

        public CCLabelAtlas(string label, string charMapFile, int itemWidth, int itemHeight, char startCharMap) : base(charMapFile, itemWidth, itemHeight, label.Length)
        {
            InitCCLabelAtlas(label, startCharMap);
        }

        public CCLabelAtlas(string label, CCTexture2D texture, int itemWidth, int itemHeight, char startCharMap) : base(texture, itemWidth, itemHeight, label.Length)
        {
            InitCCLabelAtlas(label, startCharMap);
        }

        private void InitCCLabelAtlas(string label, char startCharMap)
        {
            Debug.Assert(label != null);
            m_cMapStartChar = startCharMap;
            Text = (label);
            UpdateAtlasValues();
        }

        #endregion Constructors


        public override void UpdateAtlasValues()
        {
            if(Scene == null)
                return;

            int n = m_sString.Length;

            CCTexture2D texture = TextureAtlas.Texture;

            float textureWide = texture.PixelsWide;
            float textureHigh = texture.PixelsHigh;
            float scaleFactor = 1.0f;

            float itemWidthInPixels = ItemWidth * scaleFactor;
            float itemHeightInPixels = ItemHeight * scaleFactor;

            for (int i = 0; i < n; i++)
            {
                var a = (char) (m_sString[i] - m_cMapStartChar);
                float row = (a % ItemsPerRow);
                float col = (a / ItemsPerRow);

                #if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                // Issue #938. Don't use texStepX & texStepY
                float left          = (2 * row * itemWidthInPixels + 1) / (2 * textureWide);
                float right         = left + (itemWidthInPixels * 2 - 2) / (2 * textureWide);
                float top           = (2 * col * itemHeightInPixels + 1) / (2 * textureHigh);
                float bottom        = top + (itemHeightInPixels * 2 - 2) / (2 * textureHigh);
                #else
                float left = row * itemWidthInPixels / textureWide;
                float right = left + itemWidthInPixels / textureWide;
                float top = col * itemHeightInPixels / textureHigh;
                float bottom = top + itemHeightInPixels / textureHigh;
                #endif
                // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                CCV3F_C4B_T2F_Quad quad = new CCV3F_C4B_T2F_Quad();

                quad.TopLeft.TexCoords.U = left;
                quad.TopLeft.TexCoords.V = top;
                quad.TopRight.TexCoords.U = right;
                quad.TopRight.TexCoords.V = top;
                quad.BottomLeft.TexCoords.U = left;
                quad.BottomLeft.TexCoords.V = bottom;
                quad.BottomRight.TexCoords.U = right;
                quad.BottomRight.TexCoords.V = bottom;

                quad.BottomLeft.Vertices.X = i * ItemWidth;
                quad.BottomLeft.Vertices.Y = 0.0f;
                quad.BottomLeft.Vertices.Z = 0.0f;
                quad.BottomRight.Vertices.X = i * ItemWidth + ItemWidth;
                quad.BottomRight.Vertices.Y = 0.0f;
                quad.BottomRight.Vertices.Z = 0.0f;
                quad.TopLeft.Vertices.X = i * ItemWidth;
                quad.TopLeft.Vertices.Y = ItemHeight;
                quad.TopLeft.Vertices.Z = 0.0f;
                quad.TopRight.Vertices.X = i * ItemWidth + ItemWidth;
                quad.TopRight.Vertices.Y = ItemHeight;
                quad.TopRight.Vertices.Z = 0.0f;


                quad.TopLeft.Colors = quad.TopRight.Colors = quad.BottomLeft.Colors = quad.BottomRight.Colors =
                    new CCColor4B(DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity);

                TextureAtlas.UpdateQuad(ref quad, i);
            }
        }
    }
}