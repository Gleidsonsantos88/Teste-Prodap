﻿using FluentValidation;
using Model;
using Repository.EfCore;
using Service.Adapters;
using Service.Request;
using System;
using System.Collections.Generic;

namespace Service.TarefaService
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IValidator<CriarTarefaRequest> _criarTarefaRequestValidator;
        private readonly IValidator<AlterarTarefaRequest> _alterarTarefaRequestValidator;
        private readonly ITarefaAdapter _tarefaAdapter;

        public TarefaService(ITarefaRepository tarefaRepository,
            IValidator<CriarTarefaRequest> criarTarefaRequestValidator,
            ITarefaAdapter tarefaAdapter,
            IValidator<AlterarTarefaRequest> alterarTarefaRequestValidator)
        {
            _tarefaRepository = tarefaRepository;
            _criarTarefaRequestValidator = criarTarefaRequestValidator;
            _tarefaAdapter = tarefaAdapter;
            _alterarTarefaRequestValidator = alterarTarefaRequestValidator;
        }

        public bool Alterar(AlterarTarefaRequest tarefa)
        {
            try
            {
                _alterarTarefaRequestValidator.ValidateAndThrow(tarefa);

                _tarefaRepository.Alterar(_tarefaAdapter.ConverteAlterarTarefaRequestParaTarefa(tarefa));
                return true;
            }
            catch (ValidationException exv)
            {
                throw new ValidationException(exv.Errors);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar alterar tarefa");
            }
        }

        public Tarefa BuscarPorId(int id)
        {
            return _tarefaRepository.BuscarPorId(id);
        }

        public IEnumerable<Tarefa> BuscarPorUsuarioId(int usuarioId)
        {
            return _tarefaRepository.BuscarPorUsuarioId(usuarioId);
        }

        public bool Criar(CriarTarefaRequest tarefa)
        {
            try
            {
                _criarTarefaRequestValidator.ValidateAndThrow(tarefa);
               
                _tarefaRepository.Criar(_tarefaAdapter.ConverteCriarTarefaRequestParaTarefa(tarefa));
                return true;
            }
            catch (ValidationException exv)
            {
                throw new ValidationException(exv.Errors);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao tentar criar tarefa");
            }
        }
    }
}
