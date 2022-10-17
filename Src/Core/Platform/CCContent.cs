using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;

[assembly: InternalsVisibleTo("CocosSharp.Content.Pipeline.Importers")]
[assembly: InternalsVisibleTo("Microsoft.Xna.Framework.Content")]

namespace CocosSharp
{
	internal class CCContent
    {
        [ContentSerializer]
        public string Content { get; set; }

        /// <summary>
        /// Helper static method to load the contents of a CCContent object.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string LoadContentFile(string file)
        {
            string content = null;
            try
            {
                content = CCContentManager.SharedContentManager.Load<string>(file);
            }
            catch (Exception)
            {
                // Ignore - continue with loading as CCContent
            }
            if (content == null)
            {
                try
                {
                    var data = CCContentManager.SharedContentManager.Load<CCContent>(file);
                    if (data != null && data.Content != null)
                    {
                        content = data.Content;
                    }
                }
                catch (Exception)
                {
                }
            }
            if (content == null)
            {
                try
                {
                    var dx = CCContentManager.SharedContentManager.Load<CCContent>(file);
                    if (dx == null || dx.Content == null)
                    {
                        throw (new ContentLoadException("Could not load the contents of " + file + " as raw text."));
                    }
                    content = dx.Content;
                }
                catch (Exception ex)
                {
                    throw (new ContentLoadException("Could not load the contents of " + file + " as raw text.", ex));
                }
            }
            return (content);
        }
    }
}
