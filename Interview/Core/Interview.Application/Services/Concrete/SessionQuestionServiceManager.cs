using AutoMapper;
using Azure.Core;
using Interview.Application.Exception;
using Interview.Application.Mapper.DTO;
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.Models;
using Interview.Domain.Entities.Requests;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace Interview.Application.Services.Concrete
{
    public class SessionQuestionServiceManager : ISessionQuestionService
    {

        public readonly IMapper _mapper;

        private readonly ISessionQuestionWriteRepository _sessionQuestionWriteRepository;
        private readonly ISessionQuestionReadRepository _sessionQuestionReadRepository;

        private readonly ISessionWriteRepository _sessionWriteRepository;
        private readonly ISessionReadRepository  _sessionReadRepository;

        private readonly IQuestionWriteRepository  _questionWriteRepository;
        private readonly IQuestionReadRepository  _questionReadRepository;

        private readonly IStructureWriteRepository  _structureWriteRepository;
        private readonly IStructureReadRepository  _structureReadRepository;

        private readonly IPositionWriteRepository  _positionWriteRepository;
        private readonly IPositionReadRepository  _positionReadRepository;

        private readonly IVacancyWriteRepository  _vacancyWriteRepository;
        private readonly IVacancyReadRepository  _vacancyReadRepository;

        public SessionQuestionServiceManager(IMapper mapper, ISessionQuestionWriteRepository sessionQuestionWriteRepository, ISessionQuestionReadRepository sessionQuestionReadRepository, ISessionWriteRepository sessionWriteRepository, ISessionReadRepository sessionReadRepository, IQuestionWriteRepository questionWriteRepository, IQuestionReadRepository questionReadRepository, IStructureWriteRepository structureWriteRepository, IStructureReadRepository structureReadRepository, IPositionWriteRepository positionWriteRepository, IPositionReadRepository positionReadRepository, IVacancyWriteRepository vacancyWriteRepository, IVacancyReadRepository vacancyReadRepository)
        {
            _mapper = mapper;
            _sessionQuestionWriteRepository = sessionQuestionWriteRepository;
            _sessionQuestionReadRepository = sessionQuestionReadRepository;
            _sessionWriteRepository = sessionWriteRepository;
            _sessionReadRepository = sessionReadRepository;
            _questionWriteRepository = questionWriteRepository;
            _questionReadRepository = questionReadRepository;
            _structureWriteRepository = structureWriteRepository;
            _structureReadRepository = structureReadRepository;
            _positionWriteRepository = positionWriteRepository;
            _positionReadRepository = positionReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
        }




        #region SessionQuestion service manager

        public async Task SessionQuestionCreate(SessionQuestionDTO_forCreate model)
        {



            var entity = _mapper.Map<SessionQuestion>(model);


            var existing1 = await _sessionReadRepository.GetByIdAsync(model.SessionId.ToString(), false);
            var existing2 = await _questionReadRepository.GetByIdAsync(model.QuestionId.ToString(), false);

            if (existing1 is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Question not found");

            }

            entity = new SessionQuestion
            {
                Value = entity.Value,

                SessionId = entity.SessionId,
                QuestionId = model.QuestionId,


            };


            await _sessionQuestionWriteRepository.AddAsync(entity);

            await _sessionQuestionWriteRepository.SaveAsync();
        }

        public async Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestion()
        {
            List<SessionQuestionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("SessionQuestion not found");
            }

            return datas;
        }

        public async Task<SessionQuestionDTO_forGetandGetAll> GetSessionQuestionById(int id)
        {
            SessionQuestionDTO_forGetandGetAll item = null;


            item = _mapper.Map<SessionQuestionDTO_forGetandGetAll>(await _sessionQuestionReadRepository.GetByIdAsync(id.ToString(), false));


            if (item == null)
            {
                throw new NotFoundException("SessionQuestion not found");
            }

            return item;
        }

        public async Task<List<SessionQuestionDTO_forGetandGetAll>> GetSessionQuestionBySessionId(int sessionId)
        {
            List<SessionQuestionDTO_forGetandGetAll> datas = null;

            await Task.Run(() =>
            {
                datas = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false).Where(i=>i.SessionId== sessionId));
            });

            if (datas.Count <= 0)
            {
                throw new NotFoundException("SessionQuestion not found");
            }

            return datas;
        }

        public async Task SessionQuestionUpdate(SessionQuestionDTO_forUpdate model)
        {


            var existing = await _sessionQuestionReadRepository.GetByIdAsync(model.Id.ToString(), false);
            var existing2 = await _sessionReadRepository.GetByIdAsync(model.SessionId.ToString(), false);
            var existing3 = await _questionReadRepository.GetByIdAsync(model.QuestionId.ToString(), false);




            if (existing is null)
            {
                throw new NotFoundException("SessionQuestion not found");

            }

            if (existing2 is null)
            {
                throw new NotFoundException("Session not found");

            }

            if (existing3 is null)
            {
                throw new NotFoundException("Question not found");

            }




            var entity = _mapper.Map<SessionQuestion>(model);



            entity = new SessionQuestion
            {
                Id= entity.Id,
                Value = entity.Value,

                SessionId = entity.SessionId,
                QuestionId = entity.QuestionId,


            };

            _sessionQuestionWriteRepository.Update(entity);
            await _sessionQuestionWriteRepository.SaveAsync();

        }

        public async Task<SessionQuestionDTO_forGetandGetAll> DeleteSessionQuestionById(int id)
        {

            if (_sessionQuestionReadRepository.GetAll().Any(i => i.Id == Convert.ToInt32(id)))
            {

                await _sessionQuestionWriteRepository.RemoveByIdAsync(id.ToString());
                await _sessionQuestionWriteRepository.SaveAsync();

                return null;

            }

            else
            {
                throw new NotFoundException("SessionQuestion not found");
            }
        }

        async Task<List<QuestionDTO_forGetandGetAll>> SelectRandomItems(List<QuestionDTO_forGetandGetAll> sourceList, int count, Random random, string levelname)
        {
            if (sourceList.Count <= count)
            {
                //return sourceList;

                throw new NotFoundException($"Not enough questions on {levelname} level.");
            }

            var selectedItems = new List<QuestionDTO_forGetandGetAll>();
            var tempList = new List<QuestionDTO_forGetandGetAll>(sourceList);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = random.Next(tempList.Count);
                QuestionDTO_forGetandGetAll randomItem = tempList[randomIndex];
                selectedItems.Add(randomItem);
                tempList.RemoveAt(randomIndex);
            }

            return selectedItems;
        }


        public async Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion(RandomQuestionRequestModel model)
        {
            //int questionCount, int structureId, int positionId, int vacantionId, int sessionId

            List<QuestionDTO_forGetandGetAll> easyList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> mediumList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> hardList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> randomList = new List<QuestionDTO_forGetandGetAll>();

            Random rnd = null;

            await Task.Run(() =>
            {
                if (_mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)).Count < model.QuestionCount)
                {
                    throw new NotFoundException("Not enough questions");
                }
            });


            await Task.Run(() =>
            {
                if (!_mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)).Any(i => i.Id == model.StructureId))
                {

                    throw new NotFoundException("Structure not found");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)).Any(i => i.StructureId == model.StructureId))
                {
                    throw new NotFoundException("There are no questions about the selected structure.");
                }
            });

            await Task.Run(() =>
            {

                if (!_mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false)).Any(i => i.Id == model.PositionId))
                {
                    throw new NotFoundException("Position not found");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)).Any(i => i.Id == model.VacantionId && i.PositionId == model.PositionId && i.StructureId == model.StructureId))
                {
                    throw new NotFoundException("Vacancy not found");
                }
            });

            await Task.Run(() =>
            {
                if (!_mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)).Any(i => i.Id == model.SessionId && i.VacancyId == model.VacantionId))
                {
                    throw new NotFoundException("Session not found");
                }
            });



            if (_mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)).Any(i => i.Id == model.StructureId))
            {
                if (_mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false)).Any(i => i.Id == model.PositionId))
                {
                    if (_mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)).Any(i => i.Id == model.VacantionId && i.PositionId == model.PositionId))
                    {
                        if (_mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)).Any(i => i.Id == model.SessionId && i.VacancyId == model.VacantionId))
                        {



                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(model.PositionId.ToString(), false)).Name == "Junior")
                            {
                                rnd = new Random();

                                await Task.Run(() =>
                                {
                                    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == model.StructureId));
                                });

                                randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));


                                await Task.Run(() =>
                                {
                                    if (randomList.Count > model.QuestionCount)
                                    {
                                        var c = randomList.Count - model.QuestionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                foreach (var entity in randomList)
                                {
                                    var sessionQuestion = new SessionQuestion
                                    {
                                        QuestionId = entity.Id,
                                        SessionId = model.SessionId
                                    };

                                    await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                }


                                await _sessionQuestionWriteRepository.SaveAsync();





                                //await Task.Run(() =>
                                //{
                                //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                //    .OrderBy(x => rnd.Next())
                                //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                //    .ToList();


                                //    foreach (var id in easyrandomNumbers)
                                //    {
                                //        foreach (var item in easyList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }
                                //});


                                //await Task.Run(() =>
                                //{
                                //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();

                                //    foreach (var id in mediumrandomNumbers)
                                //    {
                                //        foreach (var item in mediumList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});


                                //await Task.Run(() =>
                                //{
                                //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();


                                //    foreach (var id in hardrandomNumbers)
                                //    {
                                //        foreach (var item in hardList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});




                            }

                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(model.PositionId.ToString(), false)).Name == "Middle")
                            {
                                rnd = new Random();


                                await Task.Run(() =>
                                {
                                    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == model.StructureId));
                                });


                                randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                await Task.Run(() =>
                                {
                                    if (randomList.Count > model.QuestionCount)
                                    {
                                        var c = randomList.Count - model.QuestionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                foreach (var entity in randomList)
                                {
                                    var sessionQuestion = new SessionQuestion
                                    {
                                        QuestionId = entity.Id,
                                        SessionId = model.SessionId
                                    };

                                    await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                }


                                await _sessionQuestionWriteRepository.SaveAsync();


                                //await Task.Run(() =>
                                //{
                                //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                //    .OrderBy(x => rnd.Next())
                                //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                //    .ToList();

                                //    foreach (var id in easyrandomNumbers)
                                //    {
                                //        foreach (var item in easyList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }
                                //});


                                //await Task.Run(() =>
                                //{
                                //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();

                                //    foreach (var id in mediumrandomNumbers)
                                //    {
                                //        foreach (var item in mediumList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});


                                //await Task.Run(() =>
                                //{
                                //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();


                                //    foreach (var id in hardrandomNumbers)
                                //    {
                                //        foreach (var item in hardList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});

                            }

                            if (_mapper.Map<PositionDTO_forGetandGetAll>(await _positionReadRepository.GetByIdAsync(model.PositionId.ToString(), false)).Name == "Senior")
                            {
                                rnd = new Random();


                                await Task.Run(() =>
                                {
                                    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == model.StructureId));
                                });

                                await Task.Run(() =>
                                {
                                    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == model.StructureId));
                                });


                                randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                                randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                                randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));



                                await Task.Run(() =>
                                {
                                    if (randomList.Count > model.QuestionCount)
                                    {
                                        var c = randomList.Count - model.QuestionCount;

                                        for (int i = 0; i < c; i++)
                                        {

                                            randomList.RemoveAt(randomList.Count - 1);

                                        }
                                    }
                                });


                                //List<SessionQuestionDTO_forGetandGetAll> sessionQuestions = null;

                                //await Task.Run(() =>
                                //{
                                //    sessionQuestions = _mapper.Map<List<SessionQuestionDTO_forGetandGetAll>>(_sessionQuestionReadRepository.GetAll(false));
                                //});


                                //foreach (var entity in sessionQuestions)
                                //{
                                //    await _sessionQuestionWriteRepository.RemoveByIdAsync(entity.Id.ToString());
                                //}

                                //await _sessionQuestionWriteRepository.SaveAsync();


                                foreach (var entity in randomList)
                                {
                                    var sessionQuestion = new SessionQuestion
                                    {
                                        QuestionId = entity.Id,
                                        SessionId = model.SessionId
                                    };

                                    await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                                }


                                await _sessionQuestionWriteRepository.SaveAsync();




                                //await Task.Run(() =>
                                //{
                                //    easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1));

                                //    var easyrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 1).Count() - 1)
                                //    .OrderBy(x => rnd.Next())
                                //    .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 20 / 100, MidpointRounding.AwayFromZero)))
                                //    .ToList();

                                //    foreach (var id in easyrandomNumbers)
                                //    {
                                //        foreach (var item in easyList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }
                                //});


                                //await Task.Run(() =>
                                //{
                                //    mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2));

                                //    var mediumrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 2).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 30 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();

                                //    foreach (var id in mediumrandomNumbers)
                                //    {
                                //        foreach (var item in mediumList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});


                                //await Task.Run(() =>
                                //{
                                //    hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3));

                                //    var hardrandomNumbers = Enumerable.Range(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).FirstOrDefault().Id, _questionReadRepository.GetAll(false).Where(i => i.LevelId == 3).Count() - 1)
                                //   .OrderBy(x => rnd.Next())
                                //   .Take(Convert.ToInt32(Math.Round(Convert.ToDouble(questionCount) * 50 / 100, MidpointRounding.AwayFromZero)))
                                //   .ToList();


                                //    foreach (var id in hardrandomNumbers)
                                //    {
                                //        foreach (var item in hardList)
                                //        {
                                //            if (item.Id == id)
                                //            {
                                //                randomList.Add(item);
                                //                break;
                                //            }
                                //        }
                                //    }

                                //});

                            }





                        }
                    }

                }

            }
            if (randomList.Count <= 0)
            {
                throw new NotFoundException("Question not found");
            }


            //if (randomList.Count < questionCount)
            //{

            //    throw new NotFoundException("Not enough questions");

            //}



            return randomList;
        }

        public async Task<List<QuestionDTO_forGetandGetAll>> GetRandomQuestion2(RandomQuestionRequestModel2 model)
        {
            //int questionCount, int structureId, int positionId, int vacantionId, int sessionId

            List<QuestionDTO_forGetandGetAll> easyList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> mediumList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> hardList = new List<QuestionDTO_forGetandGetAll>();
            List<QuestionDTO_forGetandGetAll> randomList = new List<QuestionDTO_forGetandGetAll>();

            Random rnd = null;



            IEnumerable<QuestionDTO_forGetandGetAll> questionQuery = null;
            IEnumerable<PositionDTO_forGetandGetAll> positonQuery = null;

            await Task.Run(() =>
            {

                questionQuery =
                     from q in _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false))
                     join s in _mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)) on q.StructureId equals s.Id
                     join v in _mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)) on s.Id equals v.StructureId
                     join p in _mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false)) on v.PositionId equals p.Id
                     join se in _mapper.Map<List<SessionDTO_forGetandGetAll>>(_sessionReadRepository.GetAll(false)) on v.Id equals se.VacancyId
                     where v.Id == model.VacantionId && se.Id == model.SessionId && v.StructureId == s.Id
                     select new QuestionDTO_forGetandGetAll
                     {
                         Id = q.Id,
                         Text = q.Text,
                         LevelId = q.LevelId,
                         CategoryId = q.CategoryId,
                         StructureId = q.StructureId
                     };



                positonQuery = from p in _mapper.Map<List<PositionDTO_forGetandGetAll>>(_positionReadRepository.GetAll(false))
                               join v in _mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)) on p.Id equals v.PositionId
                               where v.Id == model.VacantionId
                               select new PositionDTO_forGetandGetAll
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                               };
            });

            await Task.Run(() =>
            {
                if (_mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false)).Count < model.QuestionCount)
                {
                    throw new NotFoundException("Not enough questions");
                }
            });





            if (questionQuery.ToList().Count > 0)
            {
                if (positonQuery.ToList().Count > 0)
                {





                    if (positonQuery.ToList().FirstOrDefault().Name == "Junior")
                    {
                        rnd = new Random();

                        await Task.Run(() =>
                        {
                            easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                        randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                        randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));


                        await Task.Run(() =>
                        {
                            if (randomList.Count > model.QuestionCount)
                            {
                                var c = randomList.Count - model.QuestionCount;

                                for (int i = 0; i < c; i++)
                                {

                                    randomList.RemoveAt(randomList.Count - 1);

                                }
                            }
                        });





                        foreach (var entity in randomList)
                        {
                            var sessionQuestion = new SessionQuestion
                            {
                                QuestionId = entity.Id,
                                SessionId = model.SessionId
                            };

                            await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                        }


                        await _sessionQuestionWriteRepository.SaveAsync();










                    }

                    if (positonQuery.ToList().FirstOrDefault().Name == "Middle")
                    {
                        rnd = new Random();


                        await Task.Run(() =>
                        {
                            easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });


                        randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                        randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                        randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));





                        await Task.Run(() =>
                        {
                            if (randomList.Count > model.QuestionCount)
                            {
                                var c = randomList.Count - model.QuestionCount;

                                for (int i = 0; i < c; i++)
                                {

                                    randomList.RemoveAt(randomList.Count - 1);

                                }
                            }
                        });


                        foreach (var entity in randomList)
                        {
                            var sessionQuestion = new SessionQuestion
                            {
                                QuestionId = entity.Id,
                                SessionId = model.SessionId
                            };

                            await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                        }


                        await _sessionQuestionWriteRepository.SaveAsync();



                    }

                    if (positonQuery.ToList().FirstOrDefault().Name == "Senior")
                    {
                        rnd = new Random();


                        await Task.Run(() =>
                        {
                            easyList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 1 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            mediumList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 2 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });

                        await Task.Run(() =>
                        {
                            hardList = _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false).Where(i => i.LevelId == 3 && i.StructureId == questionQuery.ToList().FirstOrDefault().StructureId));
                        });


                        randomList.AddRange(await SelectRandomItems(easyList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 20 / 100, MidpointRounding.AwayFromZero)), rnd, "easy"));
                        randomList.AddRange(await SelectRandomItems(mediumList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 30 / 100, MidpointRounding.AwayFromZero)), rnd, "medium"));
                        randomList.AddRange(await SelectRandomItems(hardList, Convert.ToInt32(Math.Round(Convert.ToDouble(model.QuestionCount) * 50 / 100, MidpointRounding.AwayFromZero)), rnd, "difficult"));



                        await Task.Run(() =>
                        {
                            if (randomList.Count > model.QuestionCount)
                            {
                                var c = randomList.Count - model.QuestionCount;

                                for (int i = 0; i < c; i++)
                                {

                                    randomList.RemoveAt(randomList.Count - 1);

                                }
                            }
                        });



                        foreach (var entity in randomList)
                        {
                            var sessionQuestion = new SessionQuestion
                            {
                                QuestionId = entity.Id,
                                SessionId = model.SessionId
                            };

                            await _sessionQuestionWriteRepository.AddAsync(sessionQuestion);
                        }


                        await _sessionQuestionWriteRepository.SaveAsync();




                    }


                }
            }



            if (randomList.Count <= 0)
            {
                throw new NotFoundException("Question not found");
            }


   



            return randomList;
        }

        public async Task<List<QuestionDTO_forGetandGetAll>> GetAllQuestionByPage(QuestionByPageRequestModel questionByPageRequestModel)
        {


            await Task.Run(() =>
            {
                if (!_mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false)).Any(i => i.Id == questionByPageRequestModel.VacantionId))
                {
                    throw new NotFoundException("Vacancy not found");
                }
            });


            IEnumerable<QuestionDTO_forGetandGetAll> questionQuery = null;


            await Task.Run(() =>
            {

                questionQuery = from q in _mapper.Map<List<QuestionDTO_forGetandGetAll>>(_questionReadRepository.GetAll(false))
                                join s in _mapper.Map<List<StructureDTO_forGetandGetAll>>(_structureReadRepository.GetAll(false)) 
                                on q.StructureId equals s.Id
                                join v in _mapper.Map<List<VacancyDTO_forGetandGetAll>>(_vacancyReadRepository.GetAll(false))
                                on s.Id equals v.StructureId
                                where v.Id == questionByPageRequestModel.VacantionId
                                select new QuestionDTO_forGetandGetAll
                                {
                                    Id = q.Id,
                                    Text = q.Text,
                                    LevelId = q.LevelId,
                                    CategoryId = q.CategoryId,
                                    StructureId= q.StructureId,
                                };

            });

         

            

            var questions = questionQuery.ToList();

            List<QuestionDTO_forGetandGetAll> pagequestion = null;

             pagequestion = questions
            .OrderBy(q => q.Id)
            .Skip((questionByPageRequestModel.Page - 1) * questionByPageRequestModel.PageSize)
             .Take(questionByPageRequestModel.PageSize)
             .ToList();

            if (pagequestion.Count == 0)
            {

                throw new NotFoundException("Page not found");
            }

            if (pagequestion.Count < questionByPageRequestModel.PageSize)
            {

                throw new NotFoundException("The page size is not enough");
            }

            return pagequestion;
        }



        #endregion


    }


}
