namespace JollyLightsCinemaGroup.DataAccess
{
    public class DiscountCodeRepository
    {


        public bool CheckIfCodeExist(string code)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT * FROM DiscountCode WHERE Code = @code";

                command.Parameters.AddWithValue("@code", code);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public DiscountCode GetDiscountCode(string code)
        {

            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT * FROM DiscountCode WHERE Code = @code";

                command.Parameters.AddWithValue("@code", code);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DiscountCode(
                            reader.GetString(1), // Code
                            reader.GetDouble(2),  // DiscountAmount
                            reader.GetString(3),  // DiscountType
                            reader.GetDateTime(4),  // Experationdate
                            reader.IsDBNull(5) ? (int?)null : reader.GetInt32(4) // OrderId (can be NULL)
                        );
                    }
                }

                return null;
            }
        }
        public bool AddDiscountCode(DiscountCode discountcode)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO DiscountCode (Code, DiscountAmount, DiscountType, ExperationDate, OrderId)
                    VALUES (@code, @discountAmount, @discountType, @experationDate, @orderId);";

                command.Parameters.AddWithValue("@code", discountcode.Code);
                command.Parameters.AddWithValue("@discountAmount", discountcode.DiscountAmount);
                command.Parameters.AddWithValue("@discountType", discountcode.DiscountType);
                command.Parameters.AddWithValue("@experationDate", discountcode.ExperationDate);
                command.Parameters.AddWithValue("@orderId", discountcode.OrderId ?? ((object)DBNull.Value));

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }

        public bool DeleteDiscountCode(string code)
        {
            using (var connection = DatabaseManager.GetConnection())
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    DELETE FROM DiscountCode WHERE Code = @code";

                command.Parameters.AddWithValue("@code", code);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
        }
    }
}
