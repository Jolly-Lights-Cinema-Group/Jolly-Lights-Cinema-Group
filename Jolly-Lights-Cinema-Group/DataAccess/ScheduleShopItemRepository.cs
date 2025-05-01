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

        public bool RemoveScheduleShopItem(ScheduleShopItem scheduleShopItem)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM ScheduleShopItem
                    WHERE Id = @id;";

                command.Parameters.AddWithValue("@id", scheduleShopItem.Id);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
