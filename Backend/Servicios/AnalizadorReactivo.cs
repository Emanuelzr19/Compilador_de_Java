using System;
using System.Threading;
using System.Threading.Tasks;
using CompiladorJava.Modelos;

namespace CompiladorJava.Servicios;

public sealed class AnalizadorReactivo : IDisposable
{
    private readonly MotorAnalisis _motor;
    private readonly object _candado = new();
    private CancellationTokenSource? _cts;
    private int _version;
    private bool _liberado;

    public int RetardoMs { get; set; } = 350;

    public event EventHandler<ResultadoAnalisis>? ResultadoActualizado;

    public AnalizadorReactivo(MotorAnalisis? motor = null)
    {
        _motor = motor ?? new MotorAnalisis();
    }

    public void NotificarCambio(string texto)
    {
        CancellationToken token;
        int version;

        lock (_candado)
        {
            if (_liberado) return;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            token = _cts.Token;
            version = ++_version;
        }

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(RetardoMs, token).ConfigureAwait(false);
                var resultado = _motor.Analizar(texto);
                if (token.IsCancellationRequested || version != Volatile.Read(ref _version)) return;
                ResultadoActualizado?.Invoke(this, resultado);
            }
            catch (OperationCanceledException)
            {
            }
        });
    }

    public ResultadoAnalisis AnalizarAhora(string texto)
    {
        lock (_candado)
        {
            _cts?.Cancel();
            _version++;
        }
        return _motor.Analizar(texto);
    }

    public void Dispose()
    {
        lock (_candado)
        {
            _liberado = true;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
