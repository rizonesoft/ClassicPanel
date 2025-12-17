# Commit Message Standards

## Format

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

## Types

- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, missing semicolons, etc.)
- **refactor**: Code refactoring (no functional changes)
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **build**: Build system or dependency changes
- **ci**: CI/CD configuration changes
- **chore**: Other changes (maintenance tasks)
- **polish**: Polish and refinement (UI improvements, code cleanup, etc.)

## Scope (Optional)

The scope specifies the area of the codebase:
- `core`: Core business logic (CplLoader, CplInterop)
- `ui`: User interface components
- `build`: Build system
- `docs`: Documentation
- `standards`: Coding standards
- `installer`: Installer scripts

## Subject

- Use imperative, present tense: "add" not "added" or "adds"
- Don't capitalize first letter
- No period (.) at the end
- Maximum 50 characters (aim for 50, hard limit 72)
- Describe what the commit does, not why

## Body (Optional)

- Explain what and why vs. how
- Wrap at 72 characters
- Include motivation for the change
- Reference related issues or TODO items

## Footer (Optional)

- Reference issues: `Closes #123`, `Fixes #456`
- Reference TODO: `Phase: 2.1`, `Task: 2.1.3`
- Breaking changes: `BREAKING CHANGE: <description>`

## Examples

### Feature
```
feat(core): add icon extraction from CPL resources

- Implement LoadIconFromResource function
- Convert resource icon to System.Drawing.Icon
- Support multiple icon sizes (16, 24, 32, 48)

Phase: 2.3
Task: 2.3.1
```

### Bug Fix
```
fix(ui): handle null icons in ListView population

- Add null checks before adding icons to ImageList
- Display placeholder icon for missing icons
- Prevent NullReferenceException crashes

Fixes #42
```

### Documentation
```
docs: update architecture documentation

- Document CPL loading process
- Add P/Invoke interop details
- Update class diagrams
```

### Refactoring
```
refactor(core): extract CPL marshaling to separate methods

- Move CPL_INQUIRE marshaling to MarshalCplInquire
- Move CPL_NEWINQUIRE marshaling to MarshalCplNewInquire
- Improve code organization and testability
```

### Polish
```
polish(ui): improve MainWindow layout and styling

- Adjust spacing and padding
- Improve icon alignment
- Enhance visual consistency
```

### Build
```
build: configure Native AOT publish settings

- Set PublishSingleFile=true
- Configure IncludeNativeLibrariesForSelfExtract
- Update build scripts
```

## Tips

- Be consistent with the existing commit history
- Write commits as if explaining to a future developer
- One logical change per commit
- Make commits atomic (build should pass after each commit)
- Test before committing

## References
- [Conventional Commits](https://www.conventionalcommits.org/)
- [TODO.md](../TODO.md) - For phase and task references

