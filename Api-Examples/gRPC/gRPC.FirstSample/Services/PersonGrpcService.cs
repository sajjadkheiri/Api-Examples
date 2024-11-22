using Google.Protobuf.WellKnownTypes;
using gRPC.FirstSample.Protos;
using Grpc.Core;
using Microsoft.VisualBasic;

namespace gRPC.FirstSample.Services;

public class PersonGrpcService : PersonService.PersonServiceBase
{
    private List<PersonReply> _people =
    [
        new PersonReply{
            ID = 1,
            FirstName = "Sajjad",
            LastName = "Kheiri"
        },
        new PersonReply{
            ID = 2,
            FirstName = "Amir",
            LastName = "Daneshvar"
        },

            new PersonReply{
            ID = 3,
            FirstName = "Sadegh",
            LastName = "Zolghadr"
        },
            new PersonReply{
            ID = 4,
            FirstName = "Mohsen",
            LastName = "Maghare"
        },
    ];

    public override async Task CreatePerson(IAsyncStreamReader<CreatePersonRequest> requestStream, IServerStreamWriter<PersonReply> responseStream, ServerCallContext context)
    {
        await foreach (var person in requestStream.ReadAllAsync())
        {
            var peopleCount = _people.Count();

            var personReply = new PersonReply
            {
                ID = ++peopleCount,
                FirstName = person.FirstName,
                LastName = person.LastName
            };

            _people.Add(personReply);

            await responseStream.WriteAsync(personReply);
        }
    }

    public override async Task<Empty> DeletePerson(IAsyncStreamReader<PersonByIdRequest> requestStream, ServerCallContext context)
    {
        await foreach (var person in requestStream.ReadAllAsync())
        {
            var personDeleted = _people.Where(x => x.ID == person.ID).SingleOrDefault();

            if (personDeleted != null)
            {
                _people.Remove(personDeleted);
            }
        }

        return new Empty();
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<PersonReply> responseStream, ServerCallContext context)
    {
        foreach (var person in _people)
        {
            await responseStream.WriteAsync(person);
        }
    }

    public override async Task<PersonReply> GetPersonById(PersonByIdRequest request, ServerCallContext context)
    {
        var person = _people.Where(x => x.ID == request.ID).SingleOrDefault();

        if (person != null)
        {
            return person;
        }

        throw new RpcException(new Status(StatusCode.Unavailable, $"Person with Id {request.ID} not found"));
    }

    public override async Task<Empty> UpdatePerson(UpdatePersonRequest request, ServerCallContext context)
    {
        var person = _people.Where(x => x.ID == request.ID).SingleOrDefault();

        if (person != null)
        {
            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
        }

        return new Empty();
    }
}