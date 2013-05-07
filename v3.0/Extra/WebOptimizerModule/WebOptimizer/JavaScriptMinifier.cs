using System;
using System.Globalization;
using System.IO;

namespace WebOptimizer
{
	/// <summary>
	/// Minifies JavaScript text input by removing comments and whitespace.
	/// </summary>
	public class JavaScriptMinifier
	{
		private const int EOF = -1;
		private StringReader sr;
		private StringWriter sw;
		private int theA;
		private int theB;
		private int theLookahead = EOF;

		/// <summary>
		/// Minifies the specified string of JavaScript.
		/// </summary>
		/// <param name="src">The string of JavaScript to minify.</param>
		/// <returns>A minified string</returns>
		/// <author>mads</author>
		public string Minify(string src)
		{
			using (sr = new StringReader(src))
			{
				using (sw = new StringWriter(CultureInfo.InvariantCulture))
				{
					MinifyScript();
				}
			}

			return sw.ToString().Trim();
		}

		/// <summary>
		/// Copy the input to the output, deleting the characters which areinsignificant to JavaScript. Comments will be removed. 
		/// Tabs will be replaced with spaces. Carriage returns will be replaced with linefeeds. Most spaces and linefeeds will be removed.
		/// </summary>
		/// <author>mads</author>
		private void MinifyScript()
		{
			theA = '\n';
			Action(3);
			while (theA != EOF)
			{
				switch (theA)
				{
					case ' ':
						{
							if (IsAlphanum(theB))
							{
								Action(1);
							}
							else
							{
								Action(2);
							}
							break;
						}
					case '\n':
						{
							switch (theB)
							{
								case '{':
								case '[':
								case '(':
								case '+':
								case '-':
									{
										Action(1);
										break;
									}
								case ' ':
									{
										Action(3);
										break;
									}
								default:
									{
										if (IsAlphanum(theB))
										{
											Action(1);
										}
										else
										{
											Action(2);
										}
										break;
									}
							}
							break;
						}
					default:
						{
							switch (theB)
							{
								case ' ':
									{
										if (IsAlphanum(theA))
										{
											Action(1);
											break;
										}
										Action(3);
										break;
									}
								case '\n':
									{
										switch (theA)
										{
											case '}':
											case ']':
											case ')':
											case '+':
											case '-':
											case '"':
											case '\'':
												{
													Action(1);
													break;
												}
											default:
												{
													if (IsAlphanum(theA))
													{
														Action(1);
													}
													else
													{
														Action(3);
													}
													break;
												}
										}
										break;
									}
								default:
									{
										Action(1);
										break;
									}
							}
							break;
						}
				}
			}
		}

		/// <summary>
		/// action -- do something! What you do is determined by the argument:
		/// 1   Output A. Copy B to A. Get the next B.
		/// 2   Copy B to A. Get the next B. (Delete A).
		/// 3   Get the next B. (Delete B)
		/// .action treats a string as a single character. Wow!
		/// action recognizes a regular expression if it is preceded by ( or , or =.
		/// </summary>
		/// <param name="d">The action.</param>
		/// <author>mads</author>
		private void Action(int d)
		{
			if (d <= 1)
			{
				put(theA);
			}

			if (d <= 2)
			{
				theA = theB;
				if (theA == '\'' || theA == '"')
				{
					for (; ; )
					{
						put(theA);
						theA = get();
						if (theA == theB)
						{
							break;
						}

						if (theA <= '\n')
						{
							//throw new Exception(string.Format("Error: JSMIN unterminated string literal: {0}\n", sw.ToString()));
							break;
						}

						if (theA == '\\')
						{
							put(theA);
							theA = get();
						}
					}
				}
			}

			if (d <= 3)
			{
				theB = next();
				if (theB == '/' && (theA == '(' || theA == ',' || theA == '=' ||
																																																										theA == '[' || theA == '!' || theA == ':' ||
																																																										theA == '&' || theA == '|' || theA == '?' ||
																																																										theA == '{' || theA == '}' || theA == ';' ||
																																																										theA == '\n'))
				{
					put(theA);
					put(theB);
					for (; ; )
					{
						theA = get();
						if (theA == '/')
						{
							break;
						}
						else if (theA == '\\')
						{
							put(theA);
							theA = get();
						}
						else if (theA <= '\n')
						{
							throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Error: JSMIN unterminated Regular Expression literal : {0}.\n", theA));
						}

						put(theA);
					}

					theB = next();
				}
			}
		}

		/* next -- get the next character, excluding comments. peek() is used to see
																							 if a '/' is followed by a '/' or '*'.
		*/
		private int next()
		{
			int c = get();
			if (c == '/')
			{
				switch (peek())
				{
					case '/':
						{
							for (; ; )
							{
								c = get();
								if (c <= '\n')
								{
									return c;
								}
							}
						}
					case '*':
						{
							get();
							for (; ; )
							{
								switch (get())
								{
									case '*':
										{
											if (peek() == '/')
											{
												get();
												return ' ';
											}
											break;
										}
									case EOF:
										{
											throw new ArgumentException("Error: JSMIN Unterminated comment.\n");
										}
								}
							}
						}
					default:
						{
							return c;
						}
				}
			}
			return c;
		}

		/* peek -- get the next character without getting it.
		*/
		private int peek()
		{
			theLookahead = get();
			return theLookahead;
		}

		/* get -- return the next character from stdin. Watch out for lookahead. If
																							 the character is a control character, translate it to a space or
																							 linefeed.
		*/
		private int get()
		{
			int c = theLookahead;
			theLookahead = EOF;
			if (c == EOF)
			{
				c = sr.Read();
			}

			if (c >= ' ' || c == '\n' || c == EOF)
			{
				return c;
			}

			if (c == '\r')
			{
				return '\n';
			}

			return ' ';
		}

		private void put(int c)
		{
			sw.Write((char)c);
		}

		/* isAlphanum -- return true if the character is a letter, digit, underscore,
																							 dollar sign, or non-ASCII character.
		*/
		private static bool IsAlphanum(int c)
		{
			return ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') ||
																								 (c >= 'A' && c <= 'Z') || c == '_' || c == '$' || c == '\\' ||
																								 c > 126);
		}
	}
}