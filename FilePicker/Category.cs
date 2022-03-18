using System;

namespace FilePicker
{
    public class Category
    {
        public EnumCategory Cat { get; }

        public Category(FileRepresentation f)
        {
            if (f.IsPicture)
            {
                Cat = EnumCategory.Z_Picture;
                return;
            }

            switch (f.AgeInDays)
            {
                case 365:
                    Cat = EnumCategory.Y_Birthday;
                    break;

                case int tüdel when tüdel < 30:
                    Cat = EnumCategory.A_1Month;
                    break;

                case int tüdel when tüdel < 90:
                    Cat = EnumCategory.B_3Months;
                    break;

                case int tüdel when tüdel < 180:
                    Cat = EnumCategory.C_6Months;
                    break;

                case int tüdel when tüdel < 365:
                    Cat = EnumCategory.D_1Year;
                    break;

                case int tüdel when tüdel < 365 * 3:
                    Cat = EnumCategory.E_3Years;
                    break;

                case int tüdel when tüdel < 365 * 5:
                    Cat = EnumCategory.F_5Years;
                    break;

                case int tüdel when tüdel < 365 * 10:
                    Cat = EnumCategory.G_10Years;
                    break;

                case int tüdel when tüdel >= 365 * 10:
                    Cat = EnumCategory.H_10PlusYears;
                    break;

                default:
                    throw new InvalidOperationException("Could not parse Age-Category from Age");
            }
        }

        public double GetChanceToReroll()
        {
            switch (Cat)
            {
                case EnumCategory.A_1Month:
                    return 0.00;

                case EnumCategory.B_3Months:
                    return 0.05;

                case EnumCategory.C_6Months:
                    return 0.75;

                case EnumCategory.D_1Year:
                    return 0.90;

                case EnumCategory.E_3Years:
                    return 0.90;

                case EnumCategory.F_5Years:
                    return 0.95;

                case EnumCategory.G_10Years:
                    return 0.98;

                case EnumCategory.H_10PlusYears:
                    return 0.69;

                case EnumCategory.Y_Birthday:
                    return 0.00;

                case EnumCategory.Z_Picture:
                    return 0.999;

                default:
                    throw new InvalidOperationException("Unkonwn Cat");
            }
        }
    }
}