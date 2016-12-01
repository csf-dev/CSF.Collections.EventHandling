# Event-raising collections
This library introduces a wrapper for `ICollection<T>` (and implementations for `IList<T>` & `ISet<T>`) which raises events when its state is modified.

## Quick demonstration
Here is a quick demonstration of some sample code which makes use of this:

```csharp
public class Parent
{
  private EventRaisingSetWrapper<Child> _children;

  public virtual ISet<Child> Children { get { return _children.Collection; } }

  protected virtual ISet<Child> ChildrenSource {
    get { return _children.SourceCollection; }
    set { _children.SourceCollection = value; }
  }

  public Parent()
  {
    _children = new EventRaisingSetWrapper<Child>(new HashSet<Child>());
    _children.AfterAdd += (sender, e) => e.Item.Parent = this;
    _children.AfterRemove += (sender, e) => e.Item.Parent = null;
  }
}

public class Child
{
  public virtual Parent Parent
  {
    get;
    set;
  }
}
```

In this example above, we have a type `Parent` which contains a collection of its associated `Child` instances; also, each child holds a reference to its parent.
Without this, to add a new child to a parent (and back-fill its associated parent), developers would need to do something like the following:

```csharp
myParent.Children.Add(myChild);
myChild.Parent = myParent;
```

Using the event-raising collection wrapper, the back-filling happens automatically via the 'after add' event handler.
Adding the child to the parent's publicly-visible collection is enough to do both.

=== Open source license ===
All source files within this project are released as open source software, under the terms of [the MIT license].

[the MIT license]: (http://opensource.org/licenses/MIT)

This software is distributed in the hope that it will be useful, but please remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
