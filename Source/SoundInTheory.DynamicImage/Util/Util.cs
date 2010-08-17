﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Media.Imaging;

namespace SoundInTheory.DynamicImage.Util
{
	public static class Util
	{
		public static BitmapEncoder GetEncoder(DynamicImageFormat format)
		{
			switch (format)
			{
				case DynamicImageFormat.Bmp :
					return new BmpBitmapEncoder();
				case DynamicImageFormat.Gif :
					return new GifBitmapEncoder();
				case DynamicImageFormat.Jpeg :
					return new JpegBitmapEncoder();
				case DynamicImageFormat.Png :
					return new PngBitmapEncoder();
				default :
					throw new NotSupportedException();
			}
		}

		public static string MergeScript(string firstScript, string secondScript)
		{
			if (!string.IsNullOrEmpty(firstScript))
				return (firstScript + secondScript);
			if (secondScript.TrimStart(new char[0]).StartsWith("javascript:", StringComparison.Ordinal))
				return secondScript;
			return ("javascript:" + secondScript);
		}

		public static string EnsureEndWithSemiColon(string value)
		{
			if (value != null)
			{
				int length = value.Length;
				if ((length > 0) && (value[length - 1] != ';'))
					return (value + ";");
			}
			return value;
		}

		public static void SendImageToHttpResponse(HttpContext context, CompositionImage compositionImage)
		{
			context.Response.ContentType = compositionImage.Properties.MimeType;
			Composition.SaveImageStream(compositionImage, context.Response.OutputStream);
			context.Response.Flush();
		}

		/// <summary>
		/// Calculates SHA1 hash
		/// </summary>
		/// <param name="text">input string</param>
		/// <param name="encoding">Character encoding</param>
		/// <returns>SHA1 hash</returns>
		public static string CalculateShaHash(string text, Encoding encoding)
		{
			SHA256 h = SHA256.Create();
			byte[] hash = h.ComputeHash(encoding.GetBytes(text));
			// Can't use base64 hash... filesystem has case-insensitive lookup.
			// Would use base32, but too much code to bloat the resizer. Simple base16 encoding is fine
			return Base16Encode(hash);
		}

		/// <summary>
		/// Returns a lowercase hexadecimal encoding of the specified binary data
		/// </summary>
		private static string Base16Encode(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
				sb.Append(b.ToString("x").PadLeft(2, '0'));
			return sb.ToString();
		}

		/// <summary>
		/// Calculates SHA1 hash
		/// </summary>
		/// <param name="text">input string</param>
		/// <returns>SHA1 hash</returns>
		public static string CalculateShaHash(string text)
		{
			return CalculateShaHash(text, Encoding.UTF8);
		}
	}
}