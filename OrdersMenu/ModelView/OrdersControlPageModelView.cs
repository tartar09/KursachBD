using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using DataExport;

namespace OrdersMenu
{
	public class PublicOrderModel : OrderModel
	{
		public string FullName { get; set; }
		public string Name { get; set; }
	}

	public class OrdersControlPageModelView : TableEditPageModelView
	{
		public OrdersControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new OrderEditWindowModelView();
				WindowEvents.OpenWindow(typeof(OrderEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void Edit(object obj)
		{
			try
			{
				base.Edit(obj);
				var item = Database.GetOrdersList().Find(o => o.Id == SelectedItem.Id);
				var modelView = new OrderEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(OrderEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void Delete(object obj) 
		{
			try
			{
				base.Delete(obj);
				var item = Database.GetOrdersList().Find(o => o.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void LoadDataTable()
		{
			List<PublicOrderModel> ordersList = new List<PublicOrderModel>(0);

			foreach (var order in Database.GetOrdersList())
			{
				ordersList.Add(new PublicOrderModel()
				{ 
					Id = order.Id,
					Name = Database.GetBooksCatalogsList().Find(c => c.Id == order.BookCatalogId).Books.ToArray()[0].Name,
					FullName = $"{order.ReaderCard.Surname} {order.ReaderCard.Name[0]}.{order.ReaderCard.Patronymic[0]}",
					Date = order.Date,
				});
			}

			Items = new ObservableCollection<DataModel>(ordersList);
		}

	}
}
