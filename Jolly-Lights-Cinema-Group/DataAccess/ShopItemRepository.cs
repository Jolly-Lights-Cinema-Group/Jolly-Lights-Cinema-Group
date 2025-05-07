namespace JollyLightsCinemaGroup.DataAccess
{
    public class ShopItemRepository
    {
        public bool AddShopItem(ShopItem shopItem)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "SELECT COUNT(*) FROM ShopItem WHERE Name = @name;";
                command.Parameters.AddWithValue("@name", shopItem.Name);

                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    return false;
                }

                command.CommandText = @"
                    INSERT INTO ShopItem (Name, Price, Stock, MinimumAge)
                    VALUES (@name, @price, @stock, @minimumAge);";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@name", shopItem.Name);
                command.Parameters.AddWithValue("@price", shopItem.Price);
                command.Parameters.AddWithValue("@stock", shopItem.Stock);
                command.Parameters.AddWithValue("@minimumAge", shopItem.MinimumAge);

                return command.ExecuteNonQuery() > 0;
            }
        }
        public List<ShopItem> GetAllShopItems()
        {
            List<ShopItem> shopItems = new List<ShopItem>();

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Price, Stock, MinimumAge FROM ShopItem;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ShopItem shopItem = new(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetInt32(3), reader.GetInt32(4));
                        shopItems.Add(shopItem);
                    }
                }
            }
            return shopItems;
        }
        public bool ModifyShopItem(ShopItem shopItem, string? newName, string? newPrice, string? newStock, string? newMinimumAge)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();

                var updates = new List<string>();
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    updates.Add("Name = @newName");
                    parameters["@newName"] = newName;
                }

                if (!string.IsNullOrWhiteSpace(newPrice))
                {
                    if (double.TryParse(newPrice, out double validPrice))
                    {
                        updates.Add("Price = @newPrice");
                        parameters["@newPrice"] = validPrice;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newStock))
                {
                    if (int.TryParse(newStock, out int validStock))
                    {
                        updates.Add("Stock = @newStock");
                        parameters["@newStock"] = validStock;
                    }
                }

                if (!string.IsNullOrWhiteSpace(newMinimumAge))
                {
                    if (int.TryParse(newMinimumAge, out int validMinimumAge))
                    {
                        updates.Add("MinimumAge = @newMinimumAge");
                        parameters["@newMinimumAge"] = validMinimumAge;
                    }
                }

                if (updates.Count == 0)
                {
                    return false;
                }

                var command = connection.CreateCommand();
                command.CommandText = $@"
                    UPDATE ShopItem
                    SET {string.Join(", ", updates)}
                    WHERE Id = @id;";

                command.Parameters.AddWithValue("@id", shopItem.Id);

                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                return command.ExecuteNonQuery() > 0;
            }
        }

        public ShopItem? GetShopItemByName(string name)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, Name, Price, Stock, MinimumAge
                    FROM ShopItem
                    WHERE Name = @name;";

                command.Parameters.AddWithValue("@name", name);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ShopItem shopItem =  new(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2), reader.GetInt32(3), reader.GetInt32(4));
                        return shopItem;
                    }
                }
            }
            return null;
        }
    }
}
