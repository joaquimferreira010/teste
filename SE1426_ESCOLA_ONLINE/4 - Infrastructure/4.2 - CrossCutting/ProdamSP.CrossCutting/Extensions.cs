using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ProdamSP.CrossCutting
{
    public static class Extensions
    {
               
        /// <summary>
        /// Obtem valor do atributo decorador "Description" de um objeto Enum
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="enumerationValue">Atributo do Enum a qual se deseja obter o valor do Description</param>
        /// <returns>Texto contendo o conteúdo do atribudo Description</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;    
        }

        

        /// <summary>
        /// Converte uma string em um inteiro do tipo 32 bits
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static int ToInt(this string valor)
        {
            int retorno;
            if (!string.IsNullOrEmpty(valor))
            {
                bool ehNumero = false;
                foreach (char caracter in valor)
                {
                    if (char.IsNumber(caracter))
                        ehNumero = true;
                    else
                        ehNumero = false;

                    if (!ehNumero)
                        break;
                }
                if (ehNumero)
                    retorno = Convert.ToInt32(valor);
                else
                    retorno = 0;
            }
            else
                retorno = 0;
            return retorno;
        }

        /// <summary>
        /// Converte uma string em um inteiro do tipo 32 bits
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static long ToLong(this string valor)
        {
            long retorno;
            if (!string.IsNullOrEmpty(valor))
            {
                bool ehNumero = false;
                foreach (char caracter in valor)
                {
                    if (char.IsNumber(caracter))
                        ehNumero = true;
                    else
                        ehNumero = false;

                    if (!ehNumero)
                        break;
                }
                if (ehNumero)
                    retorno = Convert.ToInt64(valor);
                else
                    retorno = 0;
            }
            else
                retorno = 0;
            return retorno;
        }

        /// <summary>
        /// Converte um enum em um inteiro do tipo 32 bits.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            var retorno = Convert.ToInt32(value);
            return retorno;
        }

        /// <summary>
        /// Converte uma string em dateTime
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string valor)
        {
            return string.IsNullOrEmpty(valor) ? new DateTime() : Convert.ToDateTime(valor);
        }

        /// <summary>
        /// Converte inteiro em boleano
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>Se valor maior que 0, retorna true. Se valor menor ou igual à 0, retorna false. Se valor for null, retorna null.</returns>
        public static bool? ToBool(this int? valor)
        {
            if (valor.HasValue)
            {
                if (valor.Value > 0)
                    return true;
                else
                    return false;
            }
            else
                return null;
        }

        /// <summary>
        /// Verifica se nullable existe valor, caso positivo retorna o valor, caso negativo retorna 0
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static int GetValue(this int? valor)
        {
            return valor.HasValue ? valor.Value : 0;
        }

        /// <summary>
        /// Verifica se o inteiro nullable tem valor e se o mesmo é maior que 0
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static bool EhValido(this int? valor)
        {
            return valor.HasValue && valor.Value > 0;
        }

        /// <summary>
        /// Remove caracteres ".", "-", "/"
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string RemoverMascaras(this string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;
            var result = string.Empty;
            result = valor.Replace(".", "").Replace("-", "").Replace("/", "");

            return result;
        }
        /// <summary>
        /// Verifica se string está vazia ou nula.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static bool EhNulo(this string valor)
        {
            return string.IsNullOrWhiteSpace(valor);
        }

        /// <summary>
        /// Virifica se a lista é nula ou está vazia.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool HaRegistro<T>(this List<T> lista)
        {
            if (lista != null && lista.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Virifica se a lista é nula ou está vazia.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool HaRegistro<T>(this IList<T> lista)
        {
            if (lista != null && lista.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Virifica se a lista é nula ou está vazia.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lista"></param>
        /// <returns></returns>
        public static bool HaRegistro<T>(this IEnumerable<T> lista)
        {
            if (lista != null && lista.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Converte um inteiro em um Enumerable específico
        /// </summary>
        /// <param name=""></param>
        /// <returns> Valor do enum representante do value. </returns>
        public static T GetEnum<T>(this int value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T é do tipo enumerable");
            }
            return (T)(object)value;
        }

        /// <summary>
        /// Converte um byte em um Enumerable específico
        /// </summary>
        /// <param name=""></param>
        /// <returns> Valor do enum representante do value. </returns>
        public static T GetEnum<T>(this byte value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T é do tipo enumerable");
            }
            return (T)(object)Convert.ToInt32(value);
        }

        /// <summary>
        /// Converte um decimal em um Enumerable específico
        /// </summary>
        /// <param name=""></param>
        /// <returns> Valor do enum representante do value. </returns>
        public static T GetEnum<T>(this decimal value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T é do tipo enumerable");
            }
            return (T)(object)Convert.ToInt32(value);
        }

        /// <summary>
        /// Verifica se existe valor e se o mesmo é maior que 0
        /// </summary>
        /// <param name=""></param>
        /// <returns>Se existir valor e o mesmo for maior que 0, retorna true, caso contrário retorna false. </returns>
        public static bool Eh_Valido(this int? value)
        {
            bool retorno = false;
            if (value.HasValue && value.Value > 0)
                retorno = true;
            return retorno;
        }
        /// <summary>
        /// Verifica se valor é maior que 0
        /// </summary>
        /// <param name=""></param>
        /// <returns>Se valor for maior que 0, retorna true, caso contrário retorna false. </returns>
        public static bool EhValido(this int value)
        {
            bool retorno = false;
            if (value > 0)
                retorno = true;
            return retorno;
        }

        /// <summary>
        /// Verifica se existe valor e se o mesmo é maior que 0
        /// </summary>
        /// <param name=""></param>
        /// <returns>Se existir valor e o mesmo for maior que 0, retorna true, caso contrário retorna false. </returns>
        public static bool EhValido(this long? value)
        {
            bool retorno = false;
            if (value.HasValue && value.Value > 0)
                retorno = true;
            return retorno;
        }

        /// <summary>
        /// Verifica se a data é válida
        /// </summary>
        /// <param name=""></param>
        /// <returns>Se value for maior que a data mínima, retorna true, caso contrário retorna false. </returns>
        public static bool EhValido(this DateTime value)
        {
            bool retorno = false;
            if (value > DateTime.MinValue)
                retorno = true;
            return retorno;
        }
        /// <summary>
        /// Verifica se a data é válida
        /// </summary>
        /// <param name=""></param>
        /// <returns>Se value não for nula e for maior que a data mínima, retorna true, caso contrário retorna false. </returns>
        public static bool EhValido(this DateTime? value)
        {
            bool retorno = false;
            if (value.HasValue && value.Value > DateTime.MinValue)
                retorno = true;
            return retorno;
        }

        /// <summary>
        /// Indica se a string contém um valor numérico
        /// </summary>
        /// <param name="text">Conteúdo da string</param>
        /// <returns>Boleano indicando se a string contém valor numérico</returns>
        public static bool EhNumerico(this string text)
        {
            double test;
            return double.TryParse(text, out test);
        }

        public static string Formatar(this string valor, params object[] args)
        {
            var retorno = string.Format(valor, args);
            return retorno;
        }

        /// <summary>
        /// Verifica se o parâmetro é equivalente ao valor da string.
        /// </summary>        /// 
        /// <param name="parametro"></param>
        /// <returns>Boleano indicando se é equivalente (true) ou não equivalente (false)</returns>
        public static bool VerificarEquivalencia(this string valor, string parametro)
        {
            bool retorno = false;
            valor = valor.ToUpper();
            parametro = parametro.ToUpper();
            if (valor.Equals(parametro))
                retorno = true;
            return retorno;
        }

        public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source, params Func<TSource, object>[] keySelectors)
        {
            // initialize the table
            var seenKeysTable = keySelectors.ToDictionary(x => x, x => new HashSet<object>());

            // loop through each element in source
            foreach (var element in source)
            {
                // initialize the flag to true
                var flag = true;

                // loop through each keySelector a
                foreach (var (keySelector, hashSet) in seenKeysTable)
                {
                    // if all conditions are true
                    flag = flag && hashSet.Add(keySelector(element));
                }

                // if no duplicate key was added to table, then yield the list element
                if (flag)
                {
                    yield return element;
                }
            }
        }
    }
}
