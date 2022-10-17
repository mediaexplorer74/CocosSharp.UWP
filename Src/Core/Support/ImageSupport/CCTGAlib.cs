using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
	#region Enums and structs

	internal enum CCTGAEnum
    {
        TGA_OK,
        TGA_ERROR_FILE_OPEN,
        TGA_ERROR_READING_FILE,
        TGA_ERROR_INDEXED_COLOR,
        TGA_ERROR_MEMORY,
        TGA_ERROR_COMPRESSED_FILE,
    }

    internal struct TGAHeader
    {
		public byte Idlength;
		public byte Colourmaptype;
		public byte Datatypecode;
		public short Colourmaporigin;
		public short Colourmaplength;
		public byte Colourmapdepth;
		public short Xorigin;
		public short Yorigin;
		public short Width;
		public short Height;
		public byte Bitsperpixel;
		public byte Imagedescriptor;
    };

	#endregion Enums and structs


	internal class CCImageTGA
    {
		public byte PixelDepth;
		public short Width, Height;
		public Color[] ImageData;


		#region Constructors

		public CCImageTGA()
		{
		}

        public CCImageTGA (string fileName)
		{
            var tex = CCContentManager.SharedContentManager.Load<Texture2D>(fileName);

			Width = (short) tex.Width;
			Height = (short) tex.Height;

			ImageData = new Color[tex.Width * tex.Height];
			tex.GetData(ImageData);

			var tmp = new Color[tex.Width];
			for (int i = 0; i < tex.Height / 2; i++)
			{
				Array.Copy(ImageData, i * tex.Width, tmp, 0, tex.Width);
				Array.Copy(ImageData, (tex.Height - i - 1) * tex.Width, ImageData, i * tex.Width, tex.Width);
				Array.Copy(tmp, 0, ImageData, (tex.Height - i - 1) * tex.Width, tex.Width);
			}

		}

		#endregion Constructors
    }


    internal class CCTGAlib
    {
        static byte ReadByte(byte[] buffer, int position)
        {
            return buffer[position];
        }

        static short ReadShort(byte[] buffer, int position)
        {
            return (short)(buffer[position] | buffer[position + 1] << 8);
        }

        /// <summary>
        /// load the image header fields. We only keep those that matter!
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool LoadHeader(byte[] buffer, int bufSize, CCImageTGA psInfo)
        {
            TGAHeader header;
            int pos = 0;

			header.Idlength = ReadByte(buffer, pos);
            pos++;
			header.Colourmaptype = ReadByte(buffer, pos);
            pos++;
			header.Datatypecode = ReadByte(buffer, pos);
            pos++;
			header.Colourmaporigin = ReadShort(buffer, pos);
            pos += 2;
			header.Colourmaplength = ReadShort(buffer, pos);
            pos += 2;
			header.Colourmapdepth = ReadByte(buffer, pos);
            pos++;
			header.Xorigin = ReadShort(buffer, pos);
            pos += 2;
			header.Yorigin = ReadShort(buffer, pos);
            pos += 2;
			header.Width = ReadShort(buffer, pos);
            pos += 2;
			header.Height = ReadShort(buffer, pos);
            pos += 2;
			header.Bitsperpixel = ReadByte(buffer, pos);
            pos++;
			header.Imagedescriptor = ReadByte(buffer, pos);
            pos++;

            /*

            bool bRet = false;

            do
            {
                int step = sizeof (byte) * 2;
                if ((step + sizeof (byte)) > bufSize)
                {
                    break;
                }

                psInfo.type = ReadByte(buffer, step);

                step += sizeof (byte) * 2;
                step += sizeof (short) * 4;
                if ((step + sizeof (short) * 2 + sizeof (char)) > bufSize)
                {
                    break;
                }

                psInfo.width = ReadShort(buffer, step);
                psInfo.height = ReadShort(buffer, step + sizeof (short));
                psInfo.pixelDepth = ReadByte(buffer, step + sizeof (short) * 2);

                step += sizeof (char);
                step += sizeof (short) * 2;
                if ((step + sizeof (byte)) > bufSize)
                {
                    break;
                }

                byte cGarbage = ReadByte(buffer, step);

                psInfo.flipped = 0;
                if ((cGarbage & 0x20) != 0)
                {
                    psInfo.flipped = 1;
                }
                bRet = true;
            } while (true);
            */

            return true;
        }

        /// <summary>
        /// loads the image pixels. You shouldn't call this function directly
        /// </summary>
        /// <param name="buffer">red,green,blue pixel values</param>
        /// <returns></returns>
        public static bool LoadImageData(byte[] buffer, int bufSize, CCImageTGA psInfo)
        {
            int mode;
            int headerSkip = (1 + 2) * 6; // sizeof(char) + sizeof(short) = size of the header

            // mode equal the number of components for each pixel
			mode = psInfo.PixelDepth / 8;

            // mode=3 or 4 implies that the image is RGB(A). However TGA
            // stores it as BGR(A) so we'll have to swap R and B.
            if (mode >= 3)
            {
                int cx = 0;
                for (int i = headerSkip; i < buffer.Length; i += mode)
                {
                    psInfo.ImageData[cx].R = buffer[i + 2];
                    psInfo.ImageData[cx].G = buffer[i + 1];
                    psInfo.ImageData[cx].B = buffer[i];
                    if (mode == 4)
                    {
                        psInfo.ImageData[cx].A = buffer[i + 3];
                    }
                    else
                    {
                        psInfo.ImageData[cx].A = 255;
                    }
                }
            }
            else
            {
                return (false);
            }
            return true;

        }

        public static CCImageTGA Load(byte[] buffer)
        {
            var info = new CCImageTGA();

            LoadHeader(buffer, buffer.Length, info);

            return info;
        }

    /// <summary>
        /// this is the function to call when we want to load an image
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static CCImageTGA Load(string filename)
        {
            //int mode, total;
            CCImageTGA info = null;
            //CCFileData data = new CCFileData(pszFilename, "rb");
            //UInt64 nSize = data.Size;
            //byte[] pBuffer = data.Buffer;

            //do
            //{
            //    if (pBuffer == null)
            //    {
            //        break;
            //    }
            //    //info = malloc(sizeof(tImageTGA)) as tImageTGA;

            //    // get the file header info
            //    if (tgaLoadHeader(pBuffer, nSize, info) == null)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_MEMORY;
            //        break;
            //    }

            //    // check if the image is color indexed
            //    if (info.type == 1)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_INDEXED_COLOR;
            //        break;
            //    }

            //    // check for other types (compressed images)
            //    if ((info.type != 2) && (info.type != 3) && (info.type != 10))
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_COMPRESSED_FILE;
            //        break;
            //    }

            //    // mode equals the number of image components
            //    mode = info.pixelDepth / 8;
            //    // total is the number of unsigned chars to read
            //    total = info.height * info.width * mode;
            //    // allocate memory for image pixels
            //    // info.imageData = (char[])malloc(sizeof(unsigned char) * total);

            //    // check to make sure we have the memory required
            //    if (info.imageData == null)
            //    {
            //        info.status = (int)TGAEnum.TGA_ERROR_MEMORY;
            //        break;
            //    }

            //    bool bLoadImage = false;
            //    // finally load the image pixels
            //    if (info.type == 10)
            //    {
            //        bLoadImage = tgaLoadRLEImageData(pBuffer,nSize, info);
            //    }
            //    else
            //    {
            //        bLoadImage = tgaLoadImageData(pBuffer, nSize, info);
            //    }

            //    // check for errors when reading the pixels
            //    if (!bLoadImage)
            //    {
            //        info.status = TGAEnum.TGA_ERROR_READING_FILE;
            //        break;
            //    }
            //    info->status = TGA_OK;

            //    if (info->flipped)
            //    {
            //        tgaFlipImage(info);
            //        if (info->flipped)
            //        {
            //            info->status = TGA_ERROR_MEMORY;
            //        }
            //    }
            //} while (0);

            return info;
        }

        /// <summary>
        /// converts RGB to greyscale
        /// </summary>
        /// <param name="psInfo"></param>
        public static void RGBToGreyscale(CCImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }

        bool LoadRLEImageData(byte[] Buffer, UInt64 bufSize, CCImageTGA psInfo) 
        {
            throw new NotImplementedException();
        }
    }
}
