using System.ServiceModel.Channels;

namespace WcfCoreMessageEncoder
{
    public class CustomTextMessageEncoderFactory : MessageEncoderFactory
    {
        private MessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string mediaTypeResponse;
        private string charSet;

        internal CustomTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new CustomTextMessageEncoder(this);
        }

        internal CustomTextMessageEncoderFactory(string mediaType, string charSet,
            MessageVersion version, string mediaTypeResponse) : this(mediaType, charSet, version)
        {
            this.mediaTypeResponse = mediaTypeResponse;
        }


        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        public string MediaTypeResponse
        {
            get { return this.mediaTypeResponse; }
        }

        internal string MediaType
        {
            get
            {
                return this.mediaType;
            }
        }

        internal string CharSet
        {
            get
            {
                return this.charSet;
            }
        }


    }
}
