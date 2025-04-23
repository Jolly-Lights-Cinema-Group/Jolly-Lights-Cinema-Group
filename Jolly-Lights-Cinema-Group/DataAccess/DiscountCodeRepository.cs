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
        public void AddDiscountCode(DiscountCode discountcode)
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

                command.ExecuteNonQuery();
                Console.WriteLine("Discount code added successfully.");
            }
        }
    }
}
