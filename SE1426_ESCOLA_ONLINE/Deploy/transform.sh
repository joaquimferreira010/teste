#!/bin/bash
echo "1 - Iniciando transformacao arquivo de publicacao."

echo "Procurando arquivo $1 no caminho $2  "

V_ARQ_CONF=`find $1 -name $2`

if [ "$V_ARQ_CONF" == "" ]; then
   echo "Comando: find $1 -name $2"
   echo "Nao foi encontrado $1 no caminho $1."
   exit 1
fi

echo "2 - Finalizando transformacao arquivo de $2 no caminho $1."

sed -i "s~<base href="/" />~$3~" $V_ARQ_CONF


