using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using WcfCoreMessageEncoder;

namespace WcfCoreMessageEncoder
{
    public class CustomTextMessageBindingElement : MessageEncodingBindingElement
    {
        private MessageVersion msgVersion;
        private string mediaType;
        private string mediaTypeResponse;
        private string encoding;
        private XmlDictionaryReaderQuotas readerQuotas;
 
        CustomTextMessageBindingElement(CustomTextMessageBindingElement binding)
            : this(binding.Encoding, binding.MediaType, binding.MessageVersion, binding.MediaTypeResponse)
        {
            this.readerQuotas = new XmlDictionaryReaderQuotas();
            binding.ReaderQuotas.CopyTo(this.readerQuotas);
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType,
            MessageVersion msgVersion, string mediaTypeResponse)
        {
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            if (mediaType == null)
                throw new ArgumentNullException("mediaType");

            if (msgVersion == null)
                throw new ArgumentNullException("msgVersion");

            this.msgVersion = msgVersion;
            this.mediaType = mediaType;
            this.encoding = encoding;
            this.mediaTypeResponse = mediaTypeResponse;
            this.readerQuotas = new XmlDictionaryReaderQuotas();
        }

        public CustomTextMessageBindingElement(string encoding, string mediaType)
            : this(encoding, mediaType, System.ServiceModel.Channels.MessageVersion.Soap11, "multipart/related")
        {
        }

        public CustomTextMessageBindingElement(string encoding)
            : this(encoding, "text/xml")
        {

        }

        public CustomTextMessageBindingElement()
            : this("UTF-8")
        {
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.msgVersion;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.msgVersion = value;
            }
        }


        public string MediaType
        {
            get
            {
                return this.mediaType;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.mediaType = value;
            }
        }

        public string Encoding
        {
            get
            {
                return this.encoding;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.encoding = value;
            }
        }

        // This encoder does not enforces any quotas for the unsecure messages. The 
        // quotas are enforced for the secure portions of messages when this encoder
        // is used in a binding that is configured with security. 
        public XmlDictionaryReaderQuotas ReaderQuotas
        {
            get
            {
                return this.readerQuotas;
            }
        }

        public string MediaTypeResponse
        {
            get { return this.mediaTypeResponse; }
            set { this.mediaTypeResponse = value; }
        }

     
        public override BindingElement Clone()
        {
            return new CustomTextMessageBindingElement(this);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(XmlDictionaryReaderQuotas))
            {
                return (T)(object)this.readerQuotas;
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new CustomTextMessageEncoderFactory(this.MediaType,
                this.Encoding, this.MessageVersion, this.MediaTypeResponse);
        }
    }
}
