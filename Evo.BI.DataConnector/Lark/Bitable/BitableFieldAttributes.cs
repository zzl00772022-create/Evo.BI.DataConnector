namespace Evo.BI.Lark.Bitable;

    /// <summary>
    /// 基础字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BitableFieldAttribute : Attribute
    {
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public BitableFieldType FieldType { get; set; }
        public bool IsPrimary { get; set; }
        public string Description { get; set; }

        public BitableFieldAttribute(string fieldID, string fieldName, BitableFieldType fieldType)
        {
            FieldID = fieldID;
            FieldName = fieldName;
            FieldType = fieldType;
        }

        // 虚方法：子类可重写此方法，返回其强类型的 Property 对象
        public virtual BitablePropertyBase GetPropertyObject() => null;
    }

    /// <summary>
    /// 针对“数字”类型的特化标签
    /// </summary>
    public class BitableNumberFieldAttribute : BitableFieldAttribute
    {
        /// <summary>
        /// formatter 默认是保留1位小数，可选值有：<br/>
        /// "0": 整数<br/>
        /// "0.0": 保留1位小数<br/>
        /// "0.00": 保留2位小数<br/>
        /// "0.000": 保留3位小数<br/>
        /// "0.0000": 保留4位小数<br/>
        /// "#,##0":  千分位<br/>
        /// "#,##0.00": 千分位(2位小数)<br/>
        /// "0%": 百分比<br/>
        /// "0.00%": 百分比(2位小数)<br/>
        /// </summary>
        public string Formatter { get; set; }

        public BitableNumberFieldAttribute(string fieldID, string fieldName)
            : base(fieldID, fieldName, BitableFieldType.Number)
        {
            this.Formatter = "0.0";
        }

        public override BitablePropertyBase GetPropertyObject()
        {
            if (string.IsNullOrEmpty(Formatter)) return null;
            return new NumberProperty { Formatter = this.Formatter };
        }
    }

    /// <summary>
    /// 针对“日期”类型的特化标签
    /// </summary>
    public class BitableDateFieldAttribute : BitableFieldAttribute
    {
        /// <summary>
        /// 默认值yyyy/MM/dd
        /// </summary>
        public DateTimeFormatter Formatter { get; set; }

        public BitableDateFieldAttribute(string fieldID, string fieldName)
            : base(fieldID, fieldName, BitableFieldType.DateTime)
        {
            this.Formatter = DateTimeFormatter.yyyyMMdd_Slash;
        }

        public override BitablePropertyBase GetPropertyObject()
        {
            return new DateTimeProperty { Formatter = this.Formatter };
        }
    }

    /// <summary>
    /// 针对“货币”类型的特化标签
    /// </summary>
    public class BitableCurrencyFieldAttribute : BitableFieldAttribute
    {
        /// <summary>
        /// formatter 默认值保留2位小数，可选值：<br/>
        /// "#,##0":      整数(千分位),<br/>
        /// "#,##0.0":    保留1位小数(千分位),<br/>
        /// "#,##0.00":   保留2位小数(千分位),<br/>
        /// "#,##0.000":  保留3位小数(千分位),<br/>
        /// "#,##0.0000": 保留4位小数(千分位),<br/>
        /// </summary>
        public string Formatter { get; set; }
        public CurrencyCode CurrencyCode { get; set; }

        public BitableCurrencyFieldAttribute(string fieldID, string fieldName)
            : base(fieldID, fieldName, BitableFieldType.Currency)
        {
            this.Formatter = "#,##0.00";
            this.CurrencyCode = CurrencyCode.CNY;
        }

        public override BitablePropertyBase GetPropertyObject()
        {
            return new CurrencyProperty { Formatter = this.Formatter, CurrencyCode = this.CurrencyCode };
        }
    }
    
    /// <summary>
    /// 针对“进度”类型的特化标签
    /// </summary>
    public class BitableProgressFieldAttribute : BitableFieldAttribute
    {
        /// <summary>
        /// formatter 默认0%，可选值： <br/>
        /// 0.00%: 百分比2位小数<br/>
        /// 0.0%: 百分比1位小数,<br/>
        /// 0%:  百分比,<br/>
        /// 0.00:  2位小数<br/>
        /// 0.0:   1位小数<br/>
        /// 0:    整数<br/>
        /// </summary>
        public string Formatter { get; set; }
        /// <summary>
        /// min 默认0,min 需要小于max
        /// </summary>
        public double Min { get; set; }
        /// <summary>
        /// max 默认1，min 需要小于max
        /// </summary>
        public double Max { get; set; }
        public Color Color { get; set; }

        public BitableProgressFieldAttribute(string fieldID, string fieldName)
            : base(fieldID, fieldName, BitableFieldType.Progress)
        {
            this.Formatter = "0%";
            this.Min = 0;
            this.Max = 1;
            this.Color = Color.蓝色;
        }

        public override BitablePropertyBase GetPropertyObject()
        {
            return new ProgressProperty() { Formatter = this.Formatter, Min = this.Min, Max = this.Max, Color = this.Color };
        }
    }
    
    
    /// <summary>
    /// 针对“评分”类型的特化标签
    /// </summary>
    public class BitableRatingFieldAttribute : BitableFieldAttribute
    {
      
        public Symbol Symbol { get; set; }
      
        /// <summary>
        /// max默认5， 表示最高分，值范围是1～10 <br/>
        /// 记录的值不会校验是否在1 ~ max 之间，如果值不在1~max 之间评分无法显示
        /// </summary>
        public double Max { get; set; }

        public BitableRatingFieldAttribute(string fieldID, string fieldName)
            : base(fieldID, fieldName, BitableFieldType.Rating)
        {
            this.Symbol = Symbol.Star;
            this.Max = 5;
        }

        public override BitablePropertyBase GetPropertyObject()
        {
            return new RatingProperty() { Symbol =  this.Symbol , Max = this.Max };
        }
    }