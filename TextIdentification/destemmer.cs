#define DEBUG_STEM

using System;

namespace TextIdentification
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class destemmer
	{

		private char[] word_buffer;
		private int STR_SIZE, R1, R2;
		private int BUFFER_SIZE;
		private const int INC = 20; // I found out this is optimal word string size

		public destemmer()
		{
			SetInitState();
		}

		private void SetInitState()
		{
			R1 = R2 = -1; STR_SIZE = 0;
			BUFFER_SIZE = INC;
			word_buffer = null;
			word_buffer = new char[BUFFER_SIZE];
		}

		private void Increment()
		{
			char[] tmp_buffer = word_buffer;
			BUFFER_SIZE += INC;
			word_buffer = new char[BUFFER_SIZE];
			tmp_buffer.CopyTo(word_buffer, 0);
			tmp_buffer = null;
		}

	
		// is char vowel
		/*
		 * The following letters are vowels: 
		 *		a   e   i   o   u   y   �   �   � 
		 */
		private bool vowel(char ch)
		{
			switch(ch)
			{
				case 'a': case 'e': case 'i': case 'o':
				case 'u': case 'y': case '�': case '�':
				case '�': return true;
				default: return false;
			}
		}

		// R1 and R2 set up in the standard way
		private int DefineR(int start)
		{
			int len = STR_SIZE;
			if(start == 0)
				start = 1;

			if((start < len) && (start > 0))
			{
				for(int i = start; i < len; ++i)
				{
					if( ( !vowel(word_buffer[i]) ) && vowel( word_buffer[(i-1)] ) )
						return ( (i - start) + start + 1);
				}
			}
			return -1;
		}

		private void SetWord(string strWord)
		{
			// First, replace � by ss
			string tmp = strWord.Replace("�", "ss");
			// adjust buffer size
			while(tmp.Length > BUFFER_SIZE)
				Increment();
			STR_SIZE = tmp.Length;
			// fill in to buffer
			tmp.CopyTo(0, word_buffer, 0, STR_SIZE);
		}

		private void CutEnd(int count){	STR_SIZE -= count; }

		private bool EndsWith(string end)
		{
			if(STR_SIZE > end.Length)
			{
				int stop = STR_SIZE - end.Length - 1;
				int j = end.Length - 1;
				for(int i = (STR_SIZE -1); i > stop; --i)
				{
					if(word_buffer[i] != end[j])
						return false;
					--j;
				}
				return true;
			}
			return false;
		}

		private void preprocess()
		{
			// put u and y between vowels into upper case
			for(int i=1; i<STR_SIZE; ++i)
			{
				if( word_buffer[i] == 'u' )
				{
					if( vowel(word_buffer[(i-1)]) && vowel(word_buffer[(i+1)]) )
						word_buffer[i] = 'U';
				}
				else if( word_buffer[i] == 'y' )
				{
					if( vowel(word_buffer[(i-1)]) && vowel(word_buffer[(i+1)]) )
						word_buffer[i] = 'Y';
				}
			}
			/*
			 * R1 and R2 are first set up in the standard way,
			 * but then R1 is adjusted 
			 * so that the region before it contains at least 3 letters
			 */
			R1 = DefineR(0);
			R2 = DefineR(R1);
			if((R1 < 3) && (R1 > -1))
				R1 = 3;
		}

		/*
		 * Search for the longest among the following suffixes, 
		 *		(a) e   em   en   ern   er   es
		 *		(b) s (preceded by a valid s-ending) 
		 * and delete if in R1.
		 * (Of course the letter of the valid s-ending is not necessarily in R1) 
		 * 
		 * (For example, �ckern -> �ck, ackers -> acker, armes -> arm) 
		 */
		private void Step1()
		{
			if(R1 < 0)
				return;
			// e   em   en   ern   er   es
			if(EndsWith("ern"))
			{
				if( (STR_SIZE - R1) >= 3 )
					CutEnd(3);
			}
			else if(EndsWith("em") || EndsWith("en") || EndsWith("er") || EndsWith("es"))
			{
				if( (STR_SIZE - R1) >= 2 )
					CutEnd(2);
			}
			else if(EndsWith("e"))
			{
				if( (STR_SIZE - R1) >= 1 )
					CutEnd(1);
			}
			// b, d, f, g, h, k, l, m, n, r or t
			else if(EndsWith("bs") || EndsWith("ds") || EndsWith("fs") ||
				EndsWith("gs") || EndsWith("hs") || EndsWith("ks") ||
				EndsWith("ls") || EndsWith("ms") || EndsWith("ns") ||
				EndsWith("rs") || EndsWith("ts") )
			{
				if( (STR_SIZE - R1) >= 1 )
					CutEnd(1);
			}
		}

		/*
		 * Search for the longest among the following suffixes, 
		 *		(a) en   er   est
		 *		(b) st (preceded by a valid st-ending, itself preceded by at least 3 letters) 
		 * and delete if in R1. 
		 * 
		 * (For example, derbsten -> derbst by step 1, and derbst -> derb by step 2, since b is a valid st-ending, and is preceded by just 3 letters)
		 */
		private void Step2()
		{
			if(R1 < 0)
				return;
			 // en   er   est
			if(EndsWith("est"))
			{
				if( (STR_SIZE - R1) >= 3 )
					CutEnd(3);
			}
			else if(EndsWith("en") || EndsWith("er") )
			{
				if( (STR_SIZE - R1) >= 2 )
					CutEnd(2);
			}
			// b, d, f, g, h, k, l, m, n or t
			else if(EndsWith("bst") || EndsWith("dst") || EndsWith("fst") ||
				EndsWith("gst") || EndsWith("hst") || EndsWith("kst") ||
				EndsWith("lst") || EndsWith("mst") || EndsWith("nst") ||
				EndsWith("tst") )
			{
				// preceded by at least 3 letters
				if(STR_SIZE > 5)
				{
					if( (STR_SIZE - R1) >= 2 )
						CutEnd(2);
				}
			}
		}

		private void Step3()
		{
			if((R2 < 0) || (R1 < 0))
				return;
			/*
			 * Search for the longest among the following suffixes, 
			 * and perform the action indicated. 
			 *		end   ung 
			 * delete if in R2 
			 * if preceded by ig, delete if in R2 and not preceded by e 
			 */
			if( EndsWith("end") || EndsWith("ung") )
			{
				if( (STR_SIZE - R2) >= 3 )
					CutEnd(3);
				if( EndsWith("ig") && (word_buffer[(STR_SIZE - 3)] != 'e') )
				{
					if( (STR_SIZE - R2) >= 2 )
						CutEnd(2);
				}
			}
			/*
			 * ig   ik   isch 
			 *		delete if in R2 and not preceded by e 
			 */
			else if( (EndsWith("ig") || EndsWith("ik")) && (word_buffer[(STR_SIZE - 3)] != 'e') )
			{
				if( (STR_SIZE - R2) >= 2 )
					CutEnd(2);
			}
			else if( EndsWith("isch") && (word_buffer[(STR_SIZE - 5)] != 'e') )
			{
				if( (STR_SIZE - R2) >= 4 )
					CutEnd(4);
			}
			/*
			 * lich   heit 
			 * delete if in R2 
			 * if preceded by er or en, delete if in R1 
			 */
			else if( EndsWith("lich") || EndsWith("heit") )
			{
				CutEnd(4);
				// if preceded by er or en, delete if in R1
				if( EndsWith("en") || EndsWith("er") )
				{
					if( (STR_SIZE - R1) >= 2 )
						CutEnd(2);
					else
						STR_SIZE += 4;
				}
				else
				{
					STR_SIZE += 4;
					if( (STR_SIZE - R2) >= 4 )
						CutEnd(4);
				}
			}
			/*
			 * keit 
			 * delete if in R2 
			 * if preceded by lich or ig, delete if in R2 
			 */
			else if(EndsWith("keit"))
			{
				if( (STR_SIZE - R2) >= 4 )
					CutEnd(4);
				// if preceded by lich or ig, delete if in R2 
				if(EndsWith("ig"))
				{
					if( (STR_SIZE - R2) >= 2)
						CutEnd(2);
				}
				else if(EndsWith("lich"))
				{
					if( (STR_SIZE - R2) >= 4)
						CutEnd(4);
				}
			}
		}

		// Turn U and Y back into lower case,
		// and remove the umlaut accent from a, o and u.
		private void Finally()
		{
			for(int i=0; i<STR_SIZE; ++i)
			{
				switch(word_buffer[i])
				{
					case '�':
						word_buffer[i] = 'a'; break;
					case 'U':
						word_buffer[i] = 'u'; break;
					case '�':
						word_buffer[i] = 'u'; break;
					case 'Y':
						word_buffer[i] = 'y'; break;
					case '�':
						word_buffer[i] = 'o'; break;
					default: break;
				}
			}
		}

		public string Stem()
		{
			preprocess();
			Step1();
			Step2();
			Step3();
			Finally();
			// return stemed word
			return new string(word_buffer, 0, STR_SIZE);
		}

		public string Stem(string word)
		{
			Word = word;
			return Stem();
		}

		public string Word
		{
			get
			{
				return new string(word_buffer, 0, STR_SIZE);
			}
			set
			{
				SetInitState();
				SetWord(value);
			}
		}

	}
}
