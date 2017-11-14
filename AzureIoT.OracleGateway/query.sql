select 
	fecha_hora as FechaDB,
    hora_ticket as FechaTicket,
    cat_tabulada as Categoria,
    foto as ExisteFoto,
    recibo_emitido as ExisteRecibo,
    n_ticket as NumeroTicket,
    importe as ImportePagado,
    l1.descripcion as Procedencia,
    modo_pago.DESCRIPCION as ModoPago,
    modo_paso.descripcion as ModoPaso,
    destino as Destino,
    tarifa as Tarifa,
    turno as Turno,
    n_transito as IdTransaccion,
    l2.DESCRIPCION as Lugar
from ttransitos
inner join lugares l1 on ttransitos.procedencia = l1.ID_LUGAR
inner join modo_pago on ttransitos.MODO_PAGO = modo_pago.CODIGO
inner join modo_paso on ttransitos.MODO_PASO = modo_paso.CODIGO
inner join lugares l2 on ttransitos.id_lugar = l2.ID_LUGAR
where fecha_hora > TO_DATE(:LastDate,'MM/DD/YYYY hh24:mi:ss')
order by fecha_hora asc