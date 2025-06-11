namespace JollyLightsCinemaGroup.DataAccess
{
    public class ScheduleShopItemRepository
    {
        public bool AddScheduleShopItem(ScheduleShopItem scheduleShopItem)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO ScheduleShopItem (ShopItemId, ReservationId)
                    VALUES (@shopItemId, @reservationId);";

                command.Parameters.AddWithValue("@shopItemId", scheduleShopItem.ShopItemId);
                command.Parameters.AddWithValue("@reservationId", scheduleShopItem.ReservationId);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool RemoveScheduleShopItem(ShopItem shopItem, Reservation reservation)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM ScheduleShopItem
                    WHERE rowid = (
                        SELECT rowid FROM ScheduleShopItem
                        WHERE ShopItemId = @shopItemId AND ReservationId = @reservationId
                        LIMIT 1
                    );";

                command.Parameters.AddWithValue("@shopItemId", shopItem.Id);
                command.Parameters.AddWithValue("@reservationId", reservation.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public virtual List<ScheduleShopItem> GetScheduleShopItemByReservation(Reservation reservation)
        {
            List<ScheduleShopItem> scheduleShopItems = new List<ScheduleShopItem>();
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT Id, ShopItemId, ReservationId 
                    FROM ScheduleShopItem
                    WHERE ReservationId = @reservationId;";

                command.Parameters.AddWithValue("@reservationId", reservation.Id);


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ScheduleShopItem scheduleShopItem = new ScheduleShopItem(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2));
                        scheduleShopItems.Add(scheduleShopItem);
                    }
                }
            }
            return scheduleShopItems;
        }
    }
}
