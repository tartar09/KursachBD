using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManagers;
using WinForms = System.Windows.Forms;

namespace DataExport
{
	public class DocumentExporter
	{
		// Константы для директорий и названий файлов
		public const string DocumentDirectory = "Samples";

		private const string PenaltyDocument = "Штраф.docx";
		private const string DocumentStockSample = "Каталоги_Книг";

		private static DocumentExporter _instance;

		// Получение экземпляра класса DocumentExporter (реализация паттерна Singleton)
		public static DocumentExporter GetInstance()
		{
			if (_instance == null)
				_instance = new DocumentExporter();

			return _instance;
		}

		private DocumentExporter()
		{ }

		/// <summary>
		/// Выбор директории для сохранения документов
		/// </summary>
		/// <returns>Путь к выбранной директории</returns>
		private string GetFolder()
		{
			WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog();
			WinForms.DialogResult result = folderBrowser.ShowDialog();

			if (result != WinForms.DialogResult.OK)
				throw new Exception("Папка не выбрана");

			if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
				return folderBrowser.SelectedPath;

			throw new Exception("Папка имеет некорректный формат");
		}

		/// <summary>
		/// Создание документа штрафа в формате docx
		/// </summary>
		/// <param name="applicationModel">Модель штрафа</param>
		public void CreatePenaltyDocument(PenaltyModel penaltyModel)
		{
			string filePath = GetFolder();
			WordDocumentCreator documentCreator = new WordDocumentCreator(DocumentDirectory + "\\" + PenaltyDocument);

			string readerFullName = $"{penaltyModel.Reader.Surname} {penaltyModel.Reader.Name[0]}.{penaltyModel.Reader.Patronymic[0]}";

			// Заменяем заполнители в образце документа значениями из модели заявки
			Dictionary<string, string> items = new Dictionary<string, string>()
			{
				{ "<READER>", readerFullName },
				{ "<READER_ID>", penaltyModel.Reader.Id.ToString() },
				{ "<REASONS>", penaltyModel.Reason },
				{ "<TIME>", penaltyModel.Time.ToString() + " дней" },
				{ "<VALUE>", penaltyModel.Value.ToString() + ".руб" },
				{ "<DATE>", penaltyModel.Date },

			};

			documentCreator.CreateDocument(items, filePath);
		}

		/// <summary>
		/// Создание документа с информацией о книгах в формате Excel
		/// </summary>
		/// <param name="catalogs">Список каталогов книг </param>
		public void CreateCatalogsTable(List<BookCatalogModel> catalogs)
		{
			ExcelDocumentCreator excelDocumentCreator = new ExcelDocumentCreator();

			string[] titles = new string[] { "Id", "Название книги", "Категория", "Издание", "Дата выпуска", "Количество", "В библиотеке", };
			string[,] data = new string[catalogs.Count, titles.Length];

			// Заполняем данные для таблицы
			for (int i = 0; i < catalogs.Count; i++)
			{
				BookCatalogModel catalogModel = catalogs[i];
				BookModel bookModel = catalogModel.Books.ToArray()[0];

				data[i, 0] = catalogModel.Id.ToString();
				data[i, 1] = bookModel.Name;
				data[i, 2] = bookModel.Category.Name;
				data[i, 3] = bookModel.PublishHouseName;
				data[i, 4] = bookModel.Date;
				data[i, 5] = catalogModel.Count.ToString();
				data[i, 6] = catalogModel.AvailableCount.ToString();
			}

			string filePath = GetFolder();
			excelDocumentCreator.ExportDataToExcel(titles, data, filePath, DocumentStockSample);
		}
	}
}
