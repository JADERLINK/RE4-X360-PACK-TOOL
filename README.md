# RE4-X360-PACK-TOOL
Extract and repack RE4 XBOX360 .pack files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar arquivos .pack das versão de Xbox360;
<br> Ao extrair será gerado um arquivo de extenção .idx360pack, ele será usado para o repack.

**update: 1.0.7**
<br>Adicionado suporte a extenção .pack.yz2, pois a maioria desses arquivos não tem compressão.
<br>Aviso: arquivos com compressão NÃO são extraídos.
<br>Agora, ao arrastar arquivos sobre o programa, ele vai ficar aberto após extrair/reempacotar.
<br>Os arquivos bat funcionam iguais a antes, mas agora adicionei mais um parâmetro neles.

**Update: 1.0.6**
<br>A tool contem as mesmas funcionalidades que o RE4-UHD-PACKYZ2-TOOL version 1.0.6;
<br>(Essa é a versão de lançamento para essa tool)

## Extract

Exemplo:
<br>*RE4_X360_PACK_TOOL.exe "44000100.pack"*

* Vai gerar um arquivo de nome "44000100.pack.idx360pack";
* Vai criar uma pasta de nome "44000100";
* Na pasta vão conter as texturas, nomeadas numericamente com 4 dígitos. Ex: 0000.dds;

## Repack

Exemplo:
<br>*RE4_X360_PACK_TOOL.exe "44000100.pack.idx360pack"*

* Vai ler as imagens da pasta "44000100";
* A quantidade é definida pela numeração (então não deixe imagens faltando no meio);
* O nome do arquivo gerado é o mesmo nome do idx360pack, mas sem o .idx360pack;

**At.te: JADERLINK**
<br>2025-01-07