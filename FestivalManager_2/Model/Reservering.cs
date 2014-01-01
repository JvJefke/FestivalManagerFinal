using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FestivalManager_2.Model.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalManager_2.Model
{
    class Reservering : BaseDataAnotations
    {
        public int ID { get; set; }
        [Required]
        public string Naam { get; set; }
        [Required]
        public string Voornaam { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Ticket Ticket { get; set; }

        public static Reservering PrintGroup(ObservableCollection<Reservering> lReserveringen)
        {
            foreach(Reservering r in lReserveringen)
            {
                try
                {
                    Print(r);
                }catch(Exception ex)
                {
                    return r;
                }
            }

            return null;
        }

        private static void Print(Reservering r)
        {
            Festival f = FestivalRepository.GetFestival();

            string filename = r.ID + "_" + r.Naam + "_" + r.Voornaam + ".docx";
            File.Copy("Template.docx", filename, true);

            WordprocessingDocument newdoc = WordprocessingDocument.Open(filename, true);
            IDictionary<String, BookmarkStart> bookmarks = new Dictionary<String, BookmarkStart>();
            foreach (BookmarkStart bms in newdoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
            {
                bookmarks[bms.Name] = bms;
            }
            bookmarks["Naam"].Parent.InsertAfter<Run>(new Run(new Text(r.Naam)), bookmarks["Naam"]);
            bookmarks["Voornaam"].Parent.InsertAfter<Run>(new Run(new Text(r.Voornaam)), bookmarks["Voornaam"]);
            bookmarks["Email"].Parent.InsertAfter<Run>(new Run(new Text(r.Email)), bookmarks["Email"]);
            bookmarks["Type"].Parent.InsertAfter<Run>(new Run(new Text(r.Ticket.Type)), bookmarks["Type"]);
            bookmarks["ID"].Parent.InsertAfter<Run>(new Run(new Text(r.ID.ToString())), bookmarks["ID"]);
            bookmarks["FestivalNaam"].Parent.InsertAfter<Run>(new Run(new Text(f.Naam)), bookmarks["FestivalNaam"]);
            bookmarks["TitelType"].Parent.InsertAfter<Run>(new Run(new Text(r.Ticket.Type)), bookmarks["TitelType"]);
            bookmarks["TitelNaam"].Parent.InsertAfter<Run>(new Run(new Text(r.Naam)), bookmarks["TitelNaam"]);
            bookmarks["TitelVoornaam"].Parent.InsertAfter<Run>(new Run(new Text(r.Voornaam)), bookmarks["TitelVoornaam"]);

            Run run = new Run(new Text(r.ID.ToString()));
            RunProperties prop = new RunProperties();
            RunFonts font = new RunFonts() { Ascii = "Code39", HighAnsi = "Code39" };
            FontSize size = new FontSize() { Val = "96" };

            prop.Append(font);
            prop.Append(size);
            run.PrependChild<RunProperties>(prop);

            bookmarks["Barcode"].Parent.InsertAfter<Run>(run, bookmarks["Barcode"]);

            newdoc.Close();
        }
    }

   
}
