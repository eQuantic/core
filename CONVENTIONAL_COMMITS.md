# Conventional Commits e Versionamento Automático

Este projeto usa **GitVersion** com **Conventional Commits** para versionamento automático. A versão é incrementada automaticamente baseada no tipo de commit.

## 📋 Formato dos Commits

Use o formato padrão de Conventional Commits:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

## 🔢 Tipos de Commit e Incremento de Versão

### Patch (1.7.1 → 1.7.2)

- `fix:` - Correção de bugs
- `perf:` - Melhorias de performance
- `refactor:` - Refatoração de código
- `style:` - Mudanças de formatação
- `test:` - Adição ou correção de testes
- `docs:` - Mudanças na documentação
- `chore:` - Tarefas de manutenção

**Exemplos:**

```bash
git commit -m "fix: resolve null reference exception in DateDiff"
git commit -m "perf: optimize string concatenation in TimeFormatter"
git commit -m "docs: update README with new examples"
```

### Minor (1.7.1 → 1.8.0)

- `feat:` - Nova funcionalidade

**Exemplos:**

```bash
git commit -m "feat: add new DateRange class for period calculations"
git commit -m "feat(security): implement new encryption algorithm"
```

### Major (1.7.1 → 2.0.0)

- `feat!:` - Nova funcionalidade com breaking change
- `fix!:` - Correção com breaking change
- Qualquer commit com `BREAKING CHANGE:` no footer

**Exemplos:**

```bash
git commit -m "feat!: change DateDiff constructor signature"
git commit -m "fix!: remove deprecated methods from TimeFormatter"

# Ou usando footer:
git commit -m "feat: add new authentication method

BREAKING CHANGE: The old authenticate() method has been removed"
```

### Controle Manual de Versão

Você também pode controlar manualmente usando `+semver:` no commit:

```bash
git commit -m "docs: update changelog +semver: patch"
git commit -m "feat: add new feature +semver: minor"
git commit -m "refactor: major API changes +semver: major"
git commit -m "chore: update dependencies +semver: none"  # Não incrementa
```

## 🏷️ Tags Automáticas

- **master/main**: Versões de produção (ex: `v1.7.2`)
- **develop**: Versões alpha (ex: `v1.8.0-alpha.1`)
- **release**: Versões beta (ex: `v1.8.0-beta.1`)
- **feature**: Versões alpha (ex: `v1.8.0-alpha.1`)

## 🚀 Fluxo de Trabalho

1. **Faça commits seguindo o padrão:**

   ```bash
   git commit -m "feat: add new DateRange validation"
   ```

2. **Push para master:**

   ```bash
   git push origin master
   ```

3. **A pipeline automaticamente:**
   - Calcula a nova versão baseada nos commits
   - Atualiza a versão do pacote NuGet
   - Cria uma tag Git (ex: `v1.8.0`)
   - Publica o pacote no NuGet.org

## 📊 Exemplos de Versionamento

| Commit                             | Versão Atual | Nova Versão |
| ---------------------------------- | ------------ | ----------- |
| `fix: resolve bug`                 | 1.7.1        | 1.7.2       |
| `feat: add new method`             | 1.7.1        | 1.8.0       |
| `feat!: breaking change`           | 1.7.1        | 2.0.0       |
| `docs: update readme`              | 1.7.1        | 1.7.2       |
| `chore: update deps +semver: none` | 1.7.1        | 1.7.1       |

## 🔧 Configuração

A configuração está no arquivo `GitVersion.yml` na raiz do projeto. As principais configurações:

- **Modo**: ContinuousDeployment
- **Branch master**: Incremento Patch por padrão
- **Conventional Commits**: Habilitado
- **Prefixo de tag**: `v` (ex: v1.7.2)

## 📝 Dicas

1. **Use scopes** para organizar: `feat(security): add encryption`
2. **Seja descritivo** mas conciso na descrição
3. **Use breaking changes** com cuidado - eles geram versões major
4. **Teste localmente** antes do push para master
5. **Verifique as tags** criadas automaticamente no GitHub

## 🔍 Verificação Local

Para testar o versionamento localmente:

```bash
# Instalar GitVersion globalmente
dotnet tool install --global GitVersion.Tool

# Ver a próxima versão
dotnet-gitversion

# Ver detalhes completos
dotnet-gitversion /showvariable FullSemVer
```
