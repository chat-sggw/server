using System;
using System.Collections.Generic;
using System.Linq;
using ChatSggw.Domain.Structs;
using Neat.CQRSLite.Contract.Domain;

namespace ChatSggw.Domain.Entities.Conversation
{
    public class Conversation
    {
        public Conversation()
        {
            Messages = new List<Message>();
            Members = new List<ConversationMember>();
        }

        public Guid Id { get; set; }
        public DateTime StartDateTime { get; private set; }
        public List<ConversationMember> Members { get; private set; }
        public List<Message> Messages { get; }
        public bool IsGroupConversation { get; private set; }
        public bool IsGeoConversation { get; private set; }


        public void AddMessage(string text, Guid authorId, GeoInformation? geoStamp = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (!Members.Exists(cm => cm.UserId == authorId))
                throw new InvalidOperationException(
                    $"User with ID: {authorId} is not in the conversation"); //todo specific exception 

            if (IsGeoConversation && geoStamp == null)
            {
                throw new Exception("Your current location is needed in geo-conversation!");
            }
            var message = Message.Create(text, Id, authorId, geoStamp);
            Messages.Add(message);
            //DomainEvents.Raise(); todo
        }

        public void AddMember(Guid userId)
        {
            if (!IsGroupConversation)
                throw new InvalidOperationException($"Conversation with ID: {Id} is not group conversation");

            if (!Members.Exists(cm => cm.UserId == userId))
            {
                Members.Add(ConversationMember.Create(Id, userId));
            }
        }

        public void RemoveMember(Guid userId)
        {
            if (!IsGroupConversation)
                throw new InvalidOperationException($"Conversation with ID: {Id} is not group conversation");

            Members.RemoveAll(cm => cm.UserId == userId);
        }


        public static Conversation CreateDirectConversation(Guid firstMember, Guid secondMember)
        {
            var conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTime.Now,
                IsGroupConversation = false,
                IsGeoConversation = false
            };
            conversation.Members.Add(ConversationMember.Create(conversation.Id, firstMember));
            conversation.Members.Add(ConversationMember.Create(conversation.Id, secondMember));
            
            return conversation;
        }

        public static Conversation CreateGroupConversation(IEnumerable<Guid> members, bool isGeoConversation)
        {
            var conversation = new Conversation
            {
                Id = new Guid(),
                StartDateTime = DateTime.Now,
                IsGroupConversation = true,
            };
            conversation.Members = members
                .Select(m => ConversationMember.Create(conversation.Id, m))
                .ToList();
            conversation.IsGeoConversation = isGeoConversation;

            if (!isGeoConversation) return conversation;
            foreach (var member in conversation.Members)
            {
                member.setConversationRange(1000);
            }

            return conversation;
        }
    }
}