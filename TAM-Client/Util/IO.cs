using System;
using System.IO;
using System.Xml.Serialization;
using Android.Content;

namespace TAMClient.Util
{
	public static class IO
	{
		public static bool SaveData<T>(Context context, string fileName, T data)
		{
			try
			{
				using (Stream stream = context.OpenFileOutput(fileName, FileCreationMode.Private))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

					xmlSerializer.Serialize(stream, data);
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static T LoadData<T>(Context context, string fileName)
		{
			Java.IO.File file = context.GetFileStreamPath(fileName);

			if (file.Exists())
			{
				using (Stream openStream = context.OpenFileInput(fileName))
				{
					using (StreamReader reader = new StreamReader(openStream))
					{
						try
						{
							XmlSerializer serializer = new XmlSerializer(typeof(T));

							var loadedObject = serializer.Deserialize(reader);

							return (T)loadedObject;
						}
						catch (Exception ex)
						{
							// TODO Handle error

							return default(T);
						}
					}
				}
			}
			else
			{
				throw new Java.IO.FileNotFoundException("Could not find file " + fileName);
			}
		}
	}
}

