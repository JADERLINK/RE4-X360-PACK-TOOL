# RE4-PS3X360-PACK-TOOL
Extract and repack RE4 PS3/X360 .pack/.pack.yz2 files

**Translate from Portuguese Brazil**

Programa destinado a extrair e reempacotar arquivos .pack/.pack.yz2 das versão de Xbox360 e PS3;
<br> Ao extrair será gerado um arquivo de extenção .idxbigpack, ele será usado para o repack.
<br> Nota: so são aceitos ".pack.yz2" sem compressão;

**Last Update: V.1.0.8**

## Extract

Exemplo:
<br>*RE4_PS3X360_PACK_TOOL.exe "44000100.pack"*

* Vai gerar um arquivo de nome "44000100.pack.idxbigpack";
* Vai criar uma pasta de nome "44000100";
* Na pasta vão conter as texturas, nomeadas numericamente com 4 dígitos. Ex: 0000.dds;

## Repack

Exemplo:
<br>*RE4_PS3X360_PACK_TOOL.exe "44000100.pack.idxbigpack"*

* Vai ler as imagens da pasta "44000100";
* A quantidade é definida pela numeração (então não deixe imagens faltando no meio);
* O nome do arquivo gerado é o mesmo nome do idxbigpack, mas sem o .idxbigpack;

## Avisos:
A versão de Xbox360 só aceita imagens no formato DDS e TGA (sem compressão);
<br>A versão de PS3 só aceita imagens no formato GTF e TGA (sem compressão), e também TGA03 e TGA15 (mas essas são variantes de TGA);

## Empty and Reference

Para pular numeração, uso um arquivo com o formato .empty (não vai ter imagem nessa numeração, então não a referencie no TPL) Ex: 0001.empty
<br>Para referenciar uma imagem anterior (repeti-la), use o arquivo .reference e dentro escreva o ID da imagem. Ex: 0002.reference, e o conteúdo do arquivo vai ser: "0000" , para referenciar a textura de ID 0;

**At.te: JADERLINK**
<br>2025-02-01