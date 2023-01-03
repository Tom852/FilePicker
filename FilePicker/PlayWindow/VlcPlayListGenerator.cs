
using FilePicker.Scanner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FilePicker.PlayWindow
{
    public class VlcPlaylistGenerator
    {
        public string CreateXpfPlaylist(IEnumerable<FileRepresentation> files)
        {
            var ns = XNamespace.Get("http://xspf.org/ns/0/");
            XDocument doc =
             new XDocument(
               new XElement(ns + "playlist", new XAttribute(XNamespace.Xmlns + "vlc", "http://www.videolan.org/vlc/playlist/ns/0/"), new XAttribute("version", "1"),
                 new XElement(ns + "title", "Wiedergabeliste"),
                 new XElement(ns + "trackList", files.Select(f => new XElement(ns + "track", new XElement(ns + "location", new Uri(f.FullPath).AbsoluteUri))))
                 )
               );

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TomsFilePicker/playlist.xspf");
            doc.Save(path);
            return path;
        }
    }
}
