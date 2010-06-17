using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esath.Data.Util
{
    public class NumberSpelledOut
	{
		public static class Sum
		{
			public static StringBuilder SpellOut(decimal sum, Currency currency, StringBuilder result)
			{
				decimal integer = Math.Floor(sum);

				uint floatPart = (uint) ((sum - integer)*100);

				Number.SpellOut(integer, currency.MainMeasure, result);

				return AddCents(floatPart, currency, result);
			}


			/// <summary>
			/// Записывает SpellOut суммы в заданной валюте в <paramref name="result"/> строчными буквами.
			/// </summary>
			public static StringBuilder SpellOut(double сумма, Currency currency, StringBuilder result)
			{
				double integer = Math.Floor(сумма);

				uint floatPart = (uint) ((сумма - integer)*100);


				Number.SpellOut(integer, currency.MainMeasure, result);

				return AddCents(floatPart, currency, result);
			}


			private static StringBuilder AddCents(uint floatPart, Currency currency, StringBuilder result)
			{
				//result.Append(' ');


				// Почему-то эта строчка выполняется быстрее, чем следующая за ней закомментированная.

				//result.Append(floatPart.ToString("00"));

				//result.AppendFormat ("{0:00}", floatPart);


				//result.Append(' ');

				//result.Append(number.Согласовать(валюта.floatPartЕдиница, floatPart));


				return result;
			}


			public static string CheckSum(decimal sum)
			{
				if (sum < 0) return "Сумма должна быть неотрицательной.";


				decimal integer = Math.Floor(sum);

				decimal floatPart = (sum - integer)*100;


				if (Math.Floor(floatPart) != floatPart)
				{
					return "Сумма должна содержать не более двух цифр после запятой.";
				}


				return null;
			}



			public static string SpellOut(decimal n, Currency currency)
			{
				return Number.ApplyCaps(SpellOut(n, currency, new StringBuilder()), Заглавные.Нет);
			}


			public static string SpellOut(decimal n, Currency currency, Заглавные заглавные)
			{
				return Number.ApplyCaps(SpellOut(n, currency, new StringBuilder()), заглавные);
			}


			public static string SpellOut(double n, Currency currency)
			{
				return Number.ApplyCaps(SpellOut(n, currency, new StringBuilder()), Заглавные.Нет);
			}


			public static string SpellOut(double n, Currency currency, Заглавные заглавные)
			{
				return Number.ApplyCaps(SpellOut(n, currency, new StringBuilder()), заглавные);
			}
		}


		public static class Number
		{
			public static StringBuilder SpellOut(decimal number, IMeasureUnit mu, StringBuilder result)
			{
				string error = CheckNumber(number);

				if (error != null) throw new ArgumentException(error, "number");


				// Целочисленные версии работают в разы быстрее, чем decimal.

				if (number <= uint.MaxValue)
				{
					SpellOut((uint) number, mu, result);
				}

				else if (number <= ulong.MaxValue)
				{
					SpellOut((ulong) number, mu, result);
				}

				else
				{
					var mySb = new MyStringBuilder(result);


					decimal div1000 = Math.Floor(number/1000);

					SpellOutСтаршихКлассов(div1000, 0, mySb);

					SpellOutUnit((uint) (number - div1000*1000), mu, mySb);
				}


				return result;
			}


			public static StringBuilder SpellOut(double number, IMeasureUnit mu, StringBuilder result)
			{
				string error = CheckNumber(number);

				if (error != null) throw new ArgumentException(error, "number");


				if (number <= uint.MaxValue)
				{
					SpellOut((uint) number, mu, result);
				}

				else if (number <= ulong.MaxValue)
				{
					// SpellOut с ulong выполняется в среднем в 2 раза быстрее.

					SpellOut((ulong) number, mu, result);
				}

				else
				{
					MyStringBuilder mySb = new MyStringBuilder(result);


					double div1000 = Math.Floor(number/1000);

					SpellOutСтаршихКлассов(div1000, 0, mySb);

					SpellOutUnit((uint) (number - div1000*1000), mu, mySb);
				}


				return result;
			}




			public static StringBuilder SpellOut(ulong number, IMeasureUnit mu, StringBuilder result)
			{
				if (number <= uint.MaxValue)
				{
					SpellOut((uint) number, mu, result);
				}

				else
				{
					MyStringBuilder mySb = new MyStringBuilder(result);


					ulong div1000 = number/1000;

					SpellOutСтаршихКлассов(div1000, 0, mySb);

					SpellOutUnit((uint) (number - div1000*1000), mu, mySb);
				}


				return result;
			}


			/// <summary>
			/// Получить SpellOut числа с согласованной единицей измерения.
			/// </summary>
			/// <returns> <paramref name="result"/> </returns>
			public static StringBuilder SpellOut(uint number, IMeasureUnit mu, StringBuilder result)
			{
				MyStringBuilder mySb = new MyStringBuilder(result);


				if (number == 0)
				{
					mySb.Append("ноль");

					//mySb.Append(mu.РодМнож);
				}

				else
				{
					uint div1000 = number/1000;

					SpellOutСтаршихКлассов(div1000, 0, mySb);

					SpellOutUnit(number - div1000*1000, mu, mySb);
				}


				return result;
			}


			/// <summary>
			/// Записывает в <paramref name="sb"/> SpellOut числа, начиная с самого 
			/// старшего класса до класса с номером <paramref name="номерКласса"/>.
			/// </summary>
			/// <param name="sb"></param>
			/// <param name="number"></param>
			/// <param name="номерКласса">0 = класс тысяч, 1 = миллионов и т.д.</param>
			/// <remarks>
			/// В методе применена рекурсия, чтобы обеспечить запись в StringBuilder 
			/// в нужном порядке - от старших классов к младшим.
			/// </remarks>
			private static void SpellOutСтаршихКлассов(decimal number, int номерКласса, MyStringBuilder sb)
			{
				if (number == 0) return; // конец рекурсии


				// Записать в StringBuilder SpellOut старших классов.

				decimal div1000 = Math.Floor(number/1000);

				SpellOutСтаршихКлассов(div1000, номерКласса + 1, sb);


				uint numberДо999 = (uint) (number - div1000*1000);

				if (numberДо999 == 0) return;


				SpellOutUnit(numberДо999, Классы[номерКласса], sb);
			}


			private static void SpellOutСтаршихКлассов(double number, int номерКласса, MyStringBuilder sb)
			{
				if (number == 0) return; // конец рекурсии


				// Записать в StringBuilder SpellOut старших классов.

				double div1000 = Math.Floor(number/1000);

				SpellOutСтаршихКлассов(div1000, номерКласса + 1, sb);


				uint numberДо999 = (uint) (number - div1000*1000);

				if (numberДо999 == 0) return;


				SpellOutUnit(numberДо999, Классы[номерКласса], sb);
			}


			private static void SpellOutСтаршихКлассов(ulong number, int номерКласса, MyStringBuilder sb)
			{
				if (number == 0) return; // конец рекурсии


				// Записать в StringBuilder SpellOut старших классов.

				ulong div1000 = number/1000;

				SpellOutСтаршихКлассов(div1000, номерКласса + 1, sb);


				uint numberДо999 = (uint) (number - div1000*1000);

				if (numberДо999 == 0) return;


				SpellOutUnit(numberДо999, Классы[номерКласса], sb);
			}


			private static void SpellOutСтаршихКлассов(uint number, int номерКласса, MyStringBuilder sb)
			{
				if (number == 0) return; // конец рекурсии


				// Записать в StringBuilder SpellOut старших классов.

				uint div1000 = number/1000;

				SpellOutСтаршихКлассов(div1000, номерКласса + 1, sb);


				uint numberДо999 = number - div1000*1000;

				if (numberДо999 == 0) return;


				SpellOutUnit(numberДо999, Классы[номерКласса], sb);
			}


			private static void SpellOutUnit(uint numberLT999, IMeasureUnit unit, MyStringBuilder sb)
			{
				uint numberN = numberLT999%10;

				uint number10n = (numberLT999/10)%10;

				uint number100n = (numberLT999/100)%10;


				sb.Append(Сотни[number100n]);


				if ((numberLT999%100) != 0)
				{
					Tens[number10n].SpellOut(sb, numberN, unit.NumberForm);
				}


				// Добавить название класса в нужной форме.

				sb.Append(Normalize(unit, numberLT999));
			}


			public static string CheckNumber(decimal number)
			{
				if (number < 0)

					return "number должно быть больше или равно нулю.";


				if (number != decimal.Floor(number))

					return "number не должно содержать дробной части.";


				return null;
			}


			public static string CheckNumber(double number)
			{
				if (number < 0)

					return "number должно быть больше или равно нулю.";


				if (number != Math.Floor(number))

					return "number не должно содержать дробной части.";


				if (number > MaxDouble)
				{
					return "number должно быть не больше " + MaxDouble + ".";
				}


				return null;
			}


			public static string Normalize(IMeasureUnit measureUnit, uint number)
			{
				uint numberN = number%10;

				uint number10n = (number/10)%10;


				if (number10n == 1) return measureUnit.GenitivePlural;

				switch (numberN)
				{
					case 1:
						return measureUnit.NominativeSingle;

					case 2:
					case 3:
					case 4:
						return measureUnit.GenitiveSingle;

					default:
						return measureUnit.GenitivePlural;
				}
			}


			private static string SpellOutЦифры(uint цифра, NumberForm род)
			{
				return Цифры[цифра].SpellOut(род);
			}


			private abstract class Digit
			{
				public abstract string SpellOut(NumberForm род);
			}


			private class MutableDigit : Digit, IByForm
			{
				public MutableDigit(
					string male,
					string female,
					string mean,
					string plural)
				{
					this._male = male;

					this._female = female;

					this._mean = mean;

					this._plural = plural;
				}


				public MutableDigit(
					string single,
					string plural)
					: this(single, single, single, plural)
				{
				}


				private readonly string _male;

				private readonly string _female;

				private readonly string _mean;

				private readonly string _plural;

				#region IИзменяетсяПоРодам Members

				public string Male
				{
					get { return this._male; }
				}

				public string Female
				{
					get { return this._female; }
				}

				public string Mean
				{
					get { return this._mean; }
				}

				public string Plural
				{
					get { return this._plural; }
				}

				#endregion

				public override string SpellOut(NumberForm form)
				{
					return form.GetForm(this);
				}
			}


			private class ImmutableDigit : Digit
			{
				public ImmutableDigit(string spellOut)
				{
					this.spellOut = spellOut;
				}


				private readonly string spellOut;


				public override string SpellOut(NumberForm род)
				{
					return this.spellOut;
				}
			}


			private static readonly Digit[] Цифры = new Digit[]

			                                        	{
			                                        		null,
			                                        		new MutableDigit("один", "одна", "одно", "одни"),
			                                        		new MutableDigit("два", "две", "два", "двое"),
			                                        		new MutableDigit("три", "трое"),
			                                        		new MutableDigit("четыре", "четверо"),
			                                        		new ImmutableDigit("пять"),
			                                        		new ImmutableDigit("шесть"),
			                                        		new ImmutableDigit("семь"),
			                                        		new ImmutableDigit("восемь"),
			                                        		new ImmutableDigit("девять"),
			                                        	};

			#region Десятки

			private static readonly Десяток[] Tens = new Десяток[]

			                                         	{
			                                         		new ПервыйДесяток(),
			                                         		new ВторойДесяток(),
			                                         		new ОбычныйДесяток("двадцать"),
			                                         		new ОбычныйДесяток("тридцать"),
			                                         		new ОбычныйДесяток("сорок"),
			                                         		new ОбычныйДесяток("пятьдесят"),
			                                         		new ОбычныйДесяток("шестьдесят"),
			                                         		new ОбычныйДесяток("семьдесят"),
			                                         		new ОбычныйДесяток("восемьдесят"),
			                                         		new ОбычныйДесяток("девяносто")
			                                         	};


			private abstract class Десяток
			{
				public abstract void SpellOut(MyStringBuilder sb, uint numberЕдиниц, NumberForm род);
			}


			private class ПервыйДесяток : Десяток
			{
				public override void SpellOut(MyStringBuilder sb, uint numberЕдиниц, NumberForm род)
				{
					sb.Append(SpellOutЦифры(numberЕдиниц, род));
				}
			}


			private class ВторойДесяток : Десяток
			{
				private static readonly string[] SpellOutНаДцать = new string[]

				                                                   	{
				                                                   		"десять",
				                                                   		"одиннадцать",
				                                                   		"двенадцать",
				                                                   		"тринадцать",
				                                                   		"четырнадцать",
				                                                   		"пятнадцать",
				                                                   		"шестнадцать",
				                                                   		"семнадцать",
				                                                   		"восемнадцать",
				                                                   		"девятнадцать"
				                                                   	};


				public override void SpellOut(MyStringBuilder sb, uint numberЕдиниц, NumberForm род)
				{
					sb.Append(SpellOutНаДцать[numberЕдиниц]);
				}
			}


			private class ОбычныйДесяток : Десяток
			{
				public ОбычныйДесяток(string названиеДесятка)
				{
					this.названиеДесятка = названиеДесятка;
				}


				private readonly string названиеДесятка;


				public override void SpellOut(MyStringBuilder sb, uint numberЕдиниц, NumberForm род)
				{
					sb.Append(this.названиеДесятка);


					if (numberЕдиниц == 0)
					{
						// После "двадцать", "тридцать" и т.д. не пишут "ноль" (единиц)
					}
					else
					{
						sb.Append(SpellOutЦифры(numberЕдиниц, род));
					}
				}
			}

			#endregion

			#region Сотни

			private static readonly string[] Сотни = new string[]

			                                         	{
			                                         		null,
			                                         		"сто",
			                                         		"двести",
			                                         		"триста",
			                                         		"четыреста",
			                                         		"пятьсот",
			                                         		"шестьсот",
			                                         		"семьсот",
			                                         		"восемьсот",
			                                         		"девятьсот"
			                                         	};

			#endregion

			#region Классы

			#region КлассТысяч

			private class КлассТысяч : IMeasureUnit
			{
				public string NominativeSingle
				{
					get { return "тысяча"; }
				}

				public string GenitiveSingle
				{
					get { return "тысячи"; }
				}

				public string GenitivePlural
				{
					get { return "тысяч"; }
				}

				public NumberForm NumberForm
				{
					get { return NumberForm.Female; }
				}
			}

			#endregion

			#region Класс

			private class Класс : IMeasureUnit
			{
				private readonly string начальнаяФорма;


				public Класс(string начальнаяФорма)
				{
					this.начальнаяФорма = начальнаяФорма;
				}


				public string NominativeSingle
				{
					get { return this.начальнаяФорма; }
				}

				public string GenitiveSingle
				{
					get { return this.начальнаяФорма + "а"; }
				}

				public string GenitivePlural
				{
					get { return this.начальнаяФорма + "ов"; }
				}

				public NumberForm NumberForm
				{
					get { return NumberForm.Male; }
				}
			}

			#endregion

			/// <summary>
			/// Класс - группа из 3 цифр.  Есть классы единиц, тысяч, миллионов и т.д.
			/// </summary>
			private static readonly IMeasureUnit[] Классы = new IMeasureUnit[]

			                                                	{
			                                                		new КлассТысяч(),
			                                                		new Класс("миллион"),
			                                                		new Класс("миллиард"),
			                                                		new Класс("триллион"),
			                                                		new Класс("квадриллион"),
			                                                		new Класс("квинтиллион"),
			                                                		new Класс("секстиллион"),
			                                                		new Класс("септиллион"),
			                                                		new Класс("октиллион"),
			                                                		// Это количество классов покрывает весь диапазон типа decimal.
			                                                	};

			#endregion

			#region MaxDouble

			/// <summary>
			/// Максимальное number типа double, представимое в виде прописи.
			/// </summary>
			/// <remarks>
			/// Рассчитывается исходя из количества определённых классов.
			/// Если добавить ещё классы, оно будет автоматически увеличено.
			/// </remarks>
			public static double MaxDouble
			{
				get
				{
					if (maxDouble == 0)
					{
						maxDouble = CalcMaxDouble();
					}


					return maxDouble;
				}
			}


			private static double maxDouble = 0;


			private static double CalcMaxDouble()
			{
				double max = Math.Pow(1000, Классы.Length + 1);


				double d = 1;


				while (max - d == max)
				{
					d *= 2;
				}


				return max - d;
			}

			#endregion

			#region Вспомогательные классы

			#region Форма

			#endregion

			#region MyStringBuilder

			/// <summary>
			/// Вспомогательный класс, аналогичный <see cref="StringBuilder"/>.
			/// Между вызовами <see cref="MyStringBuilder.Append"/> вставляет пробелы.
			/// </summary>
			private class MyStringBuilder
			{
				public MyStringBuilder(StringBuilder sb)
				{
					this.sb = sb;
				}


				private readonly StringBuilder sb;

				private bool insertSpace = false;


				/// <summary>
				/// Добавляет слово <paramref name="s"/>,
				/// вставляя перед ним пробел, если нужно.
				/// </summary>
				public void Append(string s)
				{
					if (string.IsNullOrEmpty(s)) return;


					if (this.insertSpace)
					{
						this.sb.Append(' ');
					}

					else
					{
						this.insertSpace = true;
					}


					this.sb.Append(s);
				}


				public override string ToString()
				{
					return sb.ToString();
				}
			}

			#endregion

			#endregion

			#region Перегрузки метода SpellOut, возвращающие string

			/// <summary>
			/// Возвращает SpellOut числа строчными буквами.
			/// </summary>
			public static string SpellOut(decimal number, IMeasureUnit mu)
			{
				return SpellOut(number, mu, Заглавные.Нет);
			}


			/// <summary>
			/// Возвращает SpellOut числа.
			/// </summary>
			public static string SpellOut(decimal number, IMeasureUnit mu, Заглавные заглавные)
			{
				return ApplyCaps(SpellOut(number, mu, new StringBuilder()), заглавные);
			}


			/// <summary>
			/// Возвращает SpellOut числа строчными буквами.
			/// </summary>
			public static string SpellOut(double number, IMeasureUnit mu)
			{
				return SpellOut(number, mu, Заглавные.Нет);
			}


			/// <summary>
			/// Возвращает SpellOut числа.
			/// </summary>
			public static string SpellOut(double number, IMeasureUnit mu, Заглавные заглавные)
			{
				return ApplyCaps(SpellOut(number, mu, new StringBuilder()), заглавные);
			}


			/// <summary>
			/// Возвращает SpellOut числа строчными буквами.
			/// </summary>
			public static string SpellOut(ulong number, IMeasureUnit mu)
			{
				return SpellOut(number, mu, Заглавные.Нет);
			}


			/// <summary>
			/// Возвращает SpellOut числа.
			/// </summary>
			public static string SpellOut(ulong number, IMeasureUnit mu, Заглавные заглавные)
			{
				return ApplyCaps(SpellOut(number, mu, new StringBuilder()), заглавные);
			}


			/// <summary>
			/// Возвращает SpellOut числа строчными буквами.
			/// </summary>
			public static string SpellOut(uint number, IMeasureUnit mu)
			{
				return SpellOut(number, mu, Заглавные.Нет);
			}


			/// <summary>
			/// Возвращает SpellOut числа.
			/// </summary>
			public static string SpellOut(uint number, IMeasureUnit mu, Заглавные заглавные)
			{
				return ApplyCaps(SpellOut(number, mu, new StringBuilder()), заглавные);
			}


			internal static string ApplyCaps(StringBuilder sb, Заглавные заглавные)
			{
				заглавные.Применить(sb);

				return sb.ToString();
			}

			#endregion
		}


		public abstract class Заглавные
		{
			/// <summary>
			/// Применить стратегию к <paramref name="sb"/>.
			/// </summary>
			public abstract void Применить(StringBuilder sb);


			private class _ВСЕ : Заглавные
			{
				public override void Применить(StringBuilder sb)
				{
					for (int i = 0; i < sb.Length; ++i)
					{
						sb[i] = char.ToUpperInvariant(sb[i]);
					}
				}
			}


			private class _Нет : Заглавные
			{
				public override void Применить(StringBuilder sb)
				{
				}
			}


			private class _Первая : Заглавные
			{
				public override void Применить(StringBuilder sb)
				{
					sb[0] = char.ToUpperInvariant(sb[0]);
				}
			}


			public static readonly Заглавные ВСЕ = new _ВСЕ();

			public static readonly Заглавные Нет = new _Нет();

			public static readonly Заглавные Первая = new _Первая();
		}


		public class Currency
		{
			/// <summary> </summary>
			public Currency(IMeasureUnit основная, IMeasureUnit floatPart)
			{
				this.main = основная;

				this.floatPart = floatPart;
			}


			private readonly IMeasureUnit main;

			private readonly IMeasureUnit floatPart;


			public IMeasureUnit MainMeasure
			{
				get { return this.main; }
			}


			public IMeasureUnit FloatPartMeasure
			{
				get { return this.floatPart; }
			}


			public static readonly Currency Rouble = new Currency(
				new MeasureUnit(NumberForm.Male, "", "", ""),
				new MeasureUnit(NumberForm.Female, "", "", ""));


			public static readonly Currency Dollar = new Currency(
				new MeasureUnit(NumberForm.Male, "доллар США", "доллара США", "долларов США"),
				new MeasureUnit(NumberForm.Male, "цент", "цента", "центов"));


			public static readonly Currency Euro = new Currency(
				new MeasureUnit(NumberForm.Male, "евро", "евро", "евро"),
				new MeasureUnit(NumberForm.Male, "цент", "цента", "центов"));


			/// <summary>
			/// Возвращает SpellOut суммы строчными буквами.
			/// </summary>
			public string SpellOut(decimal сумма)
			{
				return Sum.SpellOut(сумма, this);
			}


			/// <summary>
			/// Возвращает SpellOut суммы строчными буквами.
			/// </summary>
			public string SpellOut(double сумма)
			{
				return Sum.SpellOut(сумма, this);
			}


			/// <summary>
			/// Возвращает SpellOut суммы.
			/// </summary>
			public string SpellOut(decimal сумма, Заглавные заглавные)
			{
				return Sum.SpellOut(сумма, this, заглавные);
			}


			/// <summary>
			/// Возвращает SpellOut суммы.
			/// </summary>
			public string SpellOut(double сумма, Заглавные заглавные)
			{
				return Sum.SpellOut(сумма, this, заглавные);
			}
		}


		public class MeasureUnit : IMeasureUnit
		{
			/// <summary> </summary>
			public MeasureUnit(NumberForm numberForm, string именЕдин, string родЕдин, string родМнож)
			{
				this.numberForm = numberForm;

				this._nominativeSingle = именЕдин;

				this._genitiveSingle = родЕдин;

				this._genitivePlural = родМнож;
			}


			private readonly NumberForm numberForm;

			private readonly string _nominativeSingle;

			private readonly string _genitiveSingle;

			private readonly string _genitivePlural;

			#region IЕдиницаИзмерения Members

			string IMeasureUnit.NominativeSingle
			{
				get { return this._nominativeSingle; }
			}


			string IMeasureUnit.GenitiveSingle
			{
				get { return this._genitiveSingle; }
			}


			string IMeasureUnit.GenitivePlural
			{
				get { return this._genitivePlural; }
			}


			NumberForm IMeasureUnit.NumberForm
			{
				get { return this.numberForm; }
			}

			#endregion
		}


		public abstract class NumberForm : IMeasureUnit
		{
			internal abstract string GetForm(IByForm слово);

			#region Рода

			private class _Male : NumberForm
			{
				internal override string GetForm(IByForm слово)
				{
					return слово.Male;
				}
			}


			private class _Female : NumberForm
			{
				internal override string GetForm(IByForm слово)
				{
					return слово.Female;
				}
			}


			private class _Mean : NumberForm
			{
				internal override string GetForm(IByForm слово)
				{
					return слово.Mean;
				}
			}


			private class _Plural : NumberForm
			{
				internal override string GetForm(IByForm слово)
				{
					return слово.Plural;
				}
			}


			public static readonly NumberForm Male = new _Male();

			public static readonly NumberForm Female = new _Female();

			public static readonly NumberForm Mean = new _Mean();

			public static readonly NumberForm Plural = new _Plural();

			#endregion

			NumberForm IMeasureUnit.NumberForm
			{
				get { return this; }
			}


			string IMeasureUnit.NominativeSingle
			{
				get { return null; }
			}


			string IMeasureUnit.GenitiveSingle
			{
				get { return null; }
			}


			string IMeasureUnit.GenitivePlural
			{
				get { return null; }
			}

		}


		internal interface IByForm
		{
			string Male { get; }

			string Female { get; }

			string Mean { get; }

			string Plural { get; }
		}


		public interface IMeasureUnit
		{
			string NominativeSingle { get; }


			string GenitiveSingle { get; }


			string GenitivePlural { get; }


			NumberForm NumberForm { get; }
		}
	}
}