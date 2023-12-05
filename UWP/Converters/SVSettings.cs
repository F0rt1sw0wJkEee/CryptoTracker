using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWP.Converters {
    public abstract class ValueConverter<TSource, TTarget> : IValueConverter {
        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TTarget? Convert(TSource? value) {
            return Convert(value, null, null);
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        public TSource? ConvertBack(TTarget? value) {
            return ConvertBack(value, null, null);
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        public object? Convert(object? value, Type? targetType, object? parameter, string? language) {
            // CastExceptions will occur when invalid value, or target type provided.
            return Convert((TSource?)value, parameter, language);
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        public object? ConvertBack(object? value, Type? targetType, object? parameter, string? language) {
            // CastExceptions will occur when invalid value, or target type provided.
            return ConvertBack((TTarget?)value, parameter, language);
        }

        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        protected virtual TTarget? Convert(TSource? value, object? parameter, string? language) {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        protected virtual TSource? ConvertBack(TTarget? value, object? parameter, string? language) {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// The base class for converting instances of type T to object and vice versa.
    /// </summary>
    public abstract class ToObjectConverter<T>
        : ValueConverter<T?, object?> {
        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        protected override object? Convert(T? value, object? parameter, string? language) {
            return value;
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        protected override T? ConvertBack(object? value, object? parameter, string? language) {
            return (T?)value;
        }
    }

    /// <summary>
    /// Converts a boolean to and from a visibility value.
    /// </summary>
    public class InverseBooleanConverter
        : ValueConverter<bool, bool> {
        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        protected override bool Convert(bool value, object? parameter, string? language) {
            return !value;
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        protected override bool ConvertBack(bool value, object? parameter, string? language) {
            return !value;
        }
    }

    public class NullToTrueConverter
        : ValueConverter<object?, bool> {
        /// <summary>
        /// Determines whether an inverse conversion should take place.
        /// </summary>
        /// <remarks>If set, the value True results in <see cref="Visibility.Collapsed"/>, and false in <see cref="Visibility.Visible"/>.</remarks>
        public bool Inverse { get; set; }

        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        protected override bool Convert(object? value, object? parameter, string? language) {
            return Inverse ? value != null : value == null;
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        protected override object? ConvertBack(bool value, object? parameter, string? language) {
            return null;
        }
    }

    public class StringNullOrWhiteSpaceToTrueConverter
        : ValueConverter<string, bool> {
        /// <summary>
        /// Determines whether an inverse conversion should take place.
        /// </summary>
        /// <remarks>If set, the value True results in <see cref="Visibility.Collapsed"/>, and false in <see cref="Visibility.Visible"/>.</remarks>
        public bool Inverse { get; set; }

        /// <summary>
        /// Converts a source value to the target type.
        /// </summary>
        protected override bool Convert(string? value, object? parameter, string? language) {
            return Inverse ? !string.IsNullOrWhiteSpace(value) : string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Converts a target value back to the source type.
        /// </summary>
        protected override string ConvertBack(bool value, object? parameter, string? language) {
            return string.Empty;
        }
    }
}
