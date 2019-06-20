<Query Kind="Expression">
  <Connection>
    <ID>eff518ad-31e8-4cc7-9f55-1235bb72ae8c</ID>
    <Server>.</Server>
    <Database>GroceryList</Database>
  </Connection>
</Query>

/* 1. 
Create a product list which indicates what products are purchased by our customers and how many times 
that product was purchased. Sort the list by most popular product by alphabetic description.*/
from x in Products
orderby x.OrderLists.Count() descending
select new {
	Product = x.Description,
	TimesPurchased = x.OrderLists.Count()
}

/* 2. 
We want a mailing list for a Valued Customers flyer that is being sent out. List the customer addresses for customers 
who have shopped at our stores. List by the store. Include the store location as well as the customer's complete address. 
Do NOT include the customer name in the results. List the customer address only once for a particular store.
from x in Stores*/
select new {
	Location = x.Location,
	Clients = (from y in x.Orders
	select new {
		address = x.Address,
		city = x.City,
		province = x.Province
	})
}

/* 3. 
Create a Daily Sales per Store request for a specified month. Sort stores by city by location. 
For Sales, show order date, number of orders, total sales without GST tax and total GST tax.*/

from x in Stores
orderby x.City, x.Location
select new {
	city = x.City,
	location = x.Location,
	sales = from y in x.Orders
			group y by y.OrderDate into yList
			select new {
				date = yList.Key,
				numberoforders = yList.Count(),
				productsales = yList.Sum(z => z.SubTotal),
				gst = yList.Sum(z => z.GST)
			}
	
}

/* 4. 
Print out all product items on a requested order (use Order #33). Group by Category and order by Product Description. 
You do not need to format money as this would be done at the presentation level. Use the QtyPicked in your calculations. 
Hint: You will need to use type casting (decimal). Use of the ternary operator will help.*/

from x in OrderLists
group x by x.Product.Category into xCat
orderby xCat.Key.Description
select new {
	Category = xCat.Key.Description,
	OrderProducts = from y in xCat
					select new {
						Product = y.Product.Description,
						Price = y.Price,
						PickedQty = y.QtyPicked,
						Discount = y.Discount,
						Subtotal = y.Price * (decimal)y.QtyPicked - y.Discount,
						Tax = y.Product.Taxable? ((decimal)y.QtyPicked * y.Price - y.Discount) * 0.05m : 0.00m,
						ExtendedPrice = ((decimal)y.QtyPicked * y.Price - y.Discount) + 
										(y.Product.Taxable? ((decimal)y.QtyPicked * y.Price - y.Discount) * 0.05m : 0.00m)
					}
}

/* 5. 
Select all orders a picker has done on a particular week (Sunday through Saturday). Group and sorted by picker. 
Sort the orders by picked date. Hint: you will need to use the join operator.
from x in Pickers select x
from x in Orders select x

from x in Pickers join y in Orders
	on x.PickerID equals y.PickerID into xPicOrd
	select xPicOrd*/

	
from x in Pickers join y in Orders
	on x.PickerID equals y.PickerID into xPicOrd
	select new {
		picker = x.LastName + ", " + x.FirstName,
		orders = (from y in x.Store.Orders
					where y.PickerID == x.PickerID
					&& y.OrderDate >= (DateTime.Parse("Dec 17, 2017"))
					&& y.OrderDate <= (DateTime.Parse("Dec 23, 2017"))
					orderby y.OrderDate
					select new {
					orderID = y.OrderID,
					date = y.PickedDate,
					})
				
}




/* 6. 
List all the products a customer (use Customer #1) has purchased and the number of times the product was purchased. 
Sort the products by number of times purchased (highest to lowest) then description.
from x in OrderLists select x*/

from x in OrderLists
	group x by x.Order.Customer into xCusOrd
	where xCusOrd.Key.CustomerID == 1
	select new {
		Customer = xCusOrd.Key.LastName + ", " + xCusOrd.Key.FirstName,
		OrderCount = xCusOrd.Key.Orders.Count(),
		Items = from y in xCusOrd
			group y by y.Product into yProdList
			orderby yProdList.Key.OrderLists.Count() descending
			select new {
				description = yProdList.Key.Description,
				timesbrought = yProdList.Key.OrderLists.Count()
			}
	}