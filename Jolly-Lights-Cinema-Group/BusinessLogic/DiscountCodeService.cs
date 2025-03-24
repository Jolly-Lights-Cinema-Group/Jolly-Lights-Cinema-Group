using JollyLightsCinemaGroup.DataAccess;
using System;
using System.Collections.Generic;

public class DiscountCodeService
{
    private readonly DiscountCodeRepository _discountCodeRepo;

    public DiscountCodeService()
    {
        _discountCodeRepo = new DiscountCodeRepository();
    }
    public void RegisterDiscountCode(string code, double discountAmount, string discountType, DateTime experationDate, int orderId)
    {
        _discountCodeRepo.AddDiscountCode(code, discountAmount, discountType, experationDate, orderId);
    }
}