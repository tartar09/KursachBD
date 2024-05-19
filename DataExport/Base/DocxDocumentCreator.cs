using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace DataExport
{
	/// <summary>
	/// Класс для создания документа Word на основе образца
	/// </summary>
	public class WordDocumentCreator
	{
		private FileInfo _fileInfo;

		/// <summary>
		/// Конструктор принимает путь к файлу-образцу и проверяет его наличие
		/// </summary>
		public WordDocumentCreator(string filePath)
		{
			if (File.Exists(filePath))
			{
				_fileInfo = new FileInfo(filePath);
			}
			else
			{
				throw new Exception("Образец документа поврежден или отсутствует");
			}
		}

		/// <summary>
		/// Метод для создания нового документа Word на основе словаря значений и сохранения его в указанную папку
		/// </summary>
		public void CreateDocument(Dictionary<string, string> items, string filePath)
		{
			Word.Application app = null;

			// Создаем новый экземпляр приложения Word
			app = new Word.Application();
			Object file = _fileInfo.FullName;
			Object missing = Type.Missing;

			// Открываем файл-образец
			app.Documents.Open(file);

			// Ищем и заменяем ключевые слова в документе на значения из словаря
			foreach (var item in items)
			{
				Word.Find find = app.Selection.Find;
				find.Text = item.Key;
				find.Replacement.Text = item.Value;

				Object wrap = Word.WdFindWrap.wdFindContinue;
				Object replace = Word.WdReplace.wdReplaceAll;

				find.Execute(FindText: Type.Missing,
					MatchCase: false,
					MatchWholeWord: false,
					MatchWildcards: false,
					MatchSoundsLike: missing,
					MatchAllWordForms: false,
					Forward: true,
					Wrap: wrap,
					Format: false,
					ReplaceWith: missing,
					Replace: replace);
			}

			// Создаем новое имя файла на основе текущей даты и сохраняем документ в указанную папку
			Object newFileName = Path.Combine(filePath, DateTime.Now.ToString("dd.MM.yyyy-HH.mm-") + _fileInfo.Name);
			app.ActiveDocument.SaveAs2(newFileName);

			// Закрываем документ и приложение Word
			app.ActiveDocument.Close();
		}

	}
}
