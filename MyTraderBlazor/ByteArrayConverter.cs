using System.Text.Json.Serialization;
using System.Text.Json;
using MyTraderBlazor.Models;

public class ByteArrayConverter : JsonConverter<TextModel>
{
    public override TextModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON payload expected to start with StartObject token.");
        }

        int id = 0;
        string text = null;
        DateTime loadDate = default;
        List<byte[]> images = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new TextModel
                {
                    Id = id,
                    Text = text,
                    LoadDate = loadDate,
                    Images = images
                };
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propName = reader.GetString();
                reader.Read();

                switch (propName)
                {
                    case "Id":
                        id = reader.GetInt32();
                        break;
                    case "Text":
                        text = reader.GetString();
                        break;
                    case "LoadDate":
                        loadDate = reader.GetDateTime();
                        break;
                    case "Images":
                        images = ReadImages(ref reader);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        throw new JsonException("Truncated file or internal error");
    }

    public override void Write(Utf8JsonWriter writer, TextModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("Text", value.Text);
        writer.WriteString("LoadDate", value.LoadDate);

        writer.WriteStartArray("Images");
        foreach (var imageBytes in value.Images)
        {
            writer.WriteStartObject();
            writer.WriteString("type", "Buffer");
            writer.WriteStartArray("data");
            foreach (var b in imageBytes)
            {
                writer.WriteNumberValue(b);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }

    private List<byte[]> ReadImages(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("JSON array expected for Images.");
        }

        var images = new List<byte[]>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return images;
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                byte[] imageBytes = null;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        images.Add(imageBytes);
                        break;
                    }

                    if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == "data")
                    {
                        imageBytes = ReadByteArray(ref reader);
                    }
                }
            }
        }

        throw new JsonException("Truncated file or internal error");
    }

    private byte[] ReadByteArray(ref Utf8JsonReader reader)
    {
        var bytes = new List<byte>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return bytes.ToArray();
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                bytes.Add(reader.GetByte());
            }
        }

        throw new JsonException("Truncated file or internal error");
    }
}